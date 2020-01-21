using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineRoulette.Domain;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OnlineRoulette.Application.AuthManager
{
    public class AuthManager : IAuthManager
    {
        private readonly JwtSettings _jwtSettings;

        public AuthManager(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Hash password with salt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string ComputeHash(string input)
        {
            SHA512Managed sha512Managed = new SHA512Managed();
            StringBuilder stringBuilder = new StringBuilder();
            byte[] crypto = sha512Managed.ComputeHash(Encoding.UTF8.GetBytes(input), 0, Encoding.UTF8.GetByteCount(input));

            foreach (byte theByte in crypto)
                stringBuilder.Append(theByte.ToString("x2"));

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generate salt 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string GenerateSalt(int size)
        {
            char symbol;
            string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder builder = new StringBuilder();
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                symbol = input[rand.Next(0, input.Length)];
                builder.Append(symbol);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Temporary password if it is required
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GenerateTemporaryPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        /// <summary>
        /// Check mail validity
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public bool IsEmailValid(string mail)
        {
            string matchEmailPattern =
                     @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                    [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				                    [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                      + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";

            if (string.IsNullOrEmpty(mail))
                return false;

            return Regex.IsMatch(mail, matchEmailPattern);
        }

        /// <summary>
        /// Generate jwt token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GenerateToken(int userId)
        {

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(_jwtSettings.AccessExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
