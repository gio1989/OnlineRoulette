using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineRoulette.Api.Filters;
using OnlineRoulette.Api.Services;
using OnlineRoulette.Api.SignalrHubs;
using OnlineRoulette.Application;
using OnlineRoulette.Application.Common.Interfaces;
using OnlineRoulette.Domain;
using OnlineRoulette.Infrastructure;
using System;
using System.IO;
using System.Text;

namespace OnlineRoulette
{
    public class Startup
    {
        public const string v1 = "version";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();

            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddSignalR();

            // Configure jwt authentication
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            var token = Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            services.AddAuthentication(x =>
       {
           x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                   ValidIssuer = token.Issuer,
                   ValidAudience = token.Audience,
                   ValidateAudience = false,
                   ValidateIssuer = false
               };
           });

            services.AddControllers();

            services.AddMvc(options =>
           {
               options.Filters.Add<ApiExceptionFilter>();

           });

            //Swagger configuration
            services.AddSwaggerGen(c =>
          {
              c.SwaggerDoc(nameof(v1), new OpenApiInfo { Title = "OnlieRoulette Api", Version = nameof(v1) });

              var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
              foreach (var xmlCommentFile in currentDir.EnumerateFiles("OnlineRoulette.*.xml"))
                  c.IncludeXmlComments(xmlCommentFile.FullName);

              c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                  In = ParameterLocation.Header,
                  Description = "Please insert JWT with Bearer into field",
                  Name = "Authorization",
                  Type = SecuritySchemeType.ApiKey
              });

              c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
            });
          });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"OnlineRoulette Api {nameof(v1)}");
            });

            // Global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<JackpotNotificationHub>("/Path goes here");

                endpoints.MapControllers();
            });
        }

    }
}
