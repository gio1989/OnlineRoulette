using AutoMapper;
using OnlineRoulette.Application.Common.Mappings;
using OnlineRoulette.Domain.Entities;
using OnlineRoulette.Domain.Enums;

namespace OnlineRoulette.Application.Common.Dtos
{
    public class BetDto : IMapFrom<BetEntity>
    {
        public BetStatus BetStatus { get; set; }
        public int SpinId { get; set; }
        public int? WinningNumber { get; set; }
        public decimal WonAmount { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BetEntity, BetDto>()
                .ForMember(d => d.WinningNumber, opt => opt.MapFrom(s => (int)s.Spin.WinningNumber));
        }

    }
}
