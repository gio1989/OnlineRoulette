using MediatR;
using OnlineRoulette.Domain.Enums;

namespace OnlineRoulette.Application.Commands
{
    public class SpinStatusChangeCommand : IRequest
    {
        public int SpinId { get; set; }
        public SpinStatus SpinStatus { get; set; }
    }
}
