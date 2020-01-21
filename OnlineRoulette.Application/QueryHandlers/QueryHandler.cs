using MediatR;
using OnlineRoulette.Application.AuthManager;
using OnlineRoulette.Application.Common.Exceptions;
using OnlineRoulette.Application.Common.Interfaces;
using OnlineRoulette.Application.Queries;
using OnlineRoulette.Domain.Entities;
using OnlineRoulette.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineRoulette.Application.QueryHandlers
{
    public class QueryHandler : IRequestHandler<LoginQuery, string>,
                                IRequestHandler<JackpotQuery, decimal>,
                                IRequestHandler<BetHistoryQuery, PagedData<BetEntity>>,
                                IRequestHandler<UserBalanceQurey, decimal?>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IAuthManager _authManager;
        private readonly ICurrentUserService _currentUserService;

        public QueryHandler(IQueryRepository queryRepository, IAuthManager authManager, ICurrentUserService currentUserService)
        {
            _queryRepository = queryRepository;
            _authManager = authManager;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || !_authManager.IsEmailValid(request.Email))
                throw new EmailNotValidException();

            var user = await _queryRepository.FindUserByUserName(request.Email)
                  ?? throw new UserNotFoundException();

            var password = _authManager.ComputeHash(request.Password + user.Salt);

            if (user.Password != password)
                throw new UserNotFoundException();

            return _authManager.GenerateToken(user.Id);
        }

        public async Task<decimal> Handle(JackpotQuery request, CancellationToken cancellationToken)
            => await _queryRepository.GetCurrentJackpot();

        public async Task<PagedData<BetEntity>> Handle(BetHistoryQuery request, CancellationToken cancellationToken)
        {
            var betHistory = await _queryRepository.GetUserBetHistory(_currentUserService.CurrentUserId, request.PageNumber, request.ItemsPerPage);
            var betHistoryCount = await _queryRepository.GetBetHistoryCount(_currentUserService.CurrentUserId);
            return new PagedData<BetEntity>(betHistory, betHistoryCount);
        }

        public async Task<decimal?> Handle(UserBalanceQurey request, CancellationToken cancellationToken)
            => await _queryRepository.GetUserBalance(request.UserId);
    }
}
