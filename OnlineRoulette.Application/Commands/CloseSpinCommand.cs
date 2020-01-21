using MediatR;

namespace OnlineRoulette.Application.Commands
{
    class CloseSpinCommand : IRequest
    {
        public int SpinId { get; set; }
    }
}
