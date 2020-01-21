using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using OnlineRoulette.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineRoulette.Application.Common.Behaviours
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUserService _currentUserService;

        public RequestLogger(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("OnlineRoulette Request: {Name} {@UserId} {@IpAddress} {@Request}",
                 requestName, _currentUserService.CurrentUserId, _currentUserService.IpAddress, request);

            await Task.FromResult("");
        }
    }
}
