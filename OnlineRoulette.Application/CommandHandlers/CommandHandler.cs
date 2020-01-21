using ge.singular.roulette;
using MediatR;
using OnlineRoulette.Application.Commands;
using OnlineRoulette.Application.Common.Dtos;
using OnlineRoulette.Application.Common.Exceptions;
using OnlineRoulette.Application.Common.Interfaces;
using OnlineRoulette.Domain.Entities;
using OnlineRoulette.Domain.Enums;
using OnlineRoulette.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineRoulette.Application.CommandHandlers
{
    public class CommandHandler : IRequestHandler<MakeBetCommand, BetDto>,
                                  IRequestHandler<CreateSpinCommand, long>,
                                  IRequestHandler<SpinStatusChangeCommand>
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IQueryRepository _queryRepository;
        private readonly ICurrentUserService _currentUserService;

        public CommandHandler(ICommandRepository commandRepository, IQueryRepository queryRepository, ICurrentUserService currentUserService)
        {
            _commandRepository = commandRepository;
            _queryRepository = queryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<BetDto> Handle(MakeBetCommand request, CancellationToken cancellationToken)
        {
            var user = await _queryRepository.FindUserById(_currentUserService.CurrentUserId)
               ?? throw new UserNotFoundException();

            string betString = Convert.ToString(request.BetString);

            CheckBet(betString, request.BetAmount, user.Balance);

            int winningNumber = new Random().Next(0, 36);

            var wonAmount = CheckBets.EstimateWin(betString, winningNumber);

            BetStatus betStatus = wonAmount > 0 ? BetStatus.Won : BetStatus.Lost;

            var jackpotAmount = await _queryRepository.GetCurrentJackpot();

            jackpotAmount += request.BetAmount * 0.01M;
            user.Balance -= request.BetAmount;

            var bet = new BetEntity
            {
                SpinId = request.SpinId,
                BetAmount = request.BetAmount,
                BetString = betString,
                IpAddress = _currentUserService.IpAddress,
                JackpotAmount = jackpotAmount,
                UserId = _currentUserService.CurrentUserId,
                WonAmount = wonAmount,
                BetStatus = betStatus,
                CreatedAt = DateTime.Now
            };
            await _commandRepository.MakeBet(bet, user.Balance, winningNumber);

            return new BetDto { SpinId = bet.SpinId, BetStatus = bet.BetStatus, WonAmount = bet.WonAmount, WinningNumber = winningNumber };
        }

        public async Task<long> Handle(CreateSpinCommand request, CancellationToken cancellationToken)
            => await _commandRepository.CreateSpin(new SpinEntity { SpinStatus = SpinStatus.Created });

        public async Task<Unit> Handle(SpinStatusChangeCommand request, CancellationToken cancellationToken)
        {
            await _commandRepository.ChangeSpinStatus(request.SpinId, request.SpinStatus);

            return Unit.Value;
        }

        #region Private Methods

        private void CheckBet(string betString, decimal betAmount, decimal currentBalance)
        {
            if (!CheckBets.IsValid(betString).getIsValid())
                throw new BetNotCorrectException();

            if (betAmount >= currentBalance)
                throw new BetNotCorrectException("Inefficient Balance");
        }

        #endregion

    }
}
