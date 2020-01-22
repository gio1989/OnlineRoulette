using AutoMapper;
using MediatR;
using OnlineRoulette.Application.AuthManager;
using OnlineRoulette.Application.Common.Dtos;
using OnlineRoulette.Application.Common.Exceptions;
using OnlineRoulette.Application.Common.Interfaces;
using OnlineRoulette.Application.Queries;
using OnlineRoulette.Domain.Entities;
using OnlineRoulette.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineRoulette.Application.QueryHandlers
{
    public class QueryHandler : IRequestHandler<LoginQuery, UserDto>,
                                IRequestHandler<JackpotQuery, decimal>,
                                IRequestHandler<BetHistoryQuery, PagedData<BetEntity>>,
                                IRequestHandler<UserBalanceQurey, decimal?>
    {
        private readonly IQueryRepository _queryRepository;
        private readonly IAuthManager _authManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;


        public QueryHandler(IQueryRepository queryRepository, IAuthManager authManager, ICurrentUserService currentUserService, IMapper mapper)
        {
            _queryRepository = queryRepository;
            _authManager = authManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || !_authManager.IsEmailValid(request.Email))
                throw new EmailNotValidException();

            var user = await _queryRepository.FindUserByUserName(request.Email)
                  ?? throw new UserNotFoundException();

            var password = _authManager.ComputeHash(request.Password + user.Salt);

            if (user.Password != password)
                throw new UserNotFoundException();

            var userInfo = _mapper.Map<UserDto>(user);

            userInfo.Token = _authManager.GenerateToken(user.Id);

            return userInfo;
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
