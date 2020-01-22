using AutoMapper;
using OnlineRoulette.Application.Common.Mappings;
using OnlineRoulette.Domain.Entities;

namespace OnlineRoulette.Application.Common.Dtos
{
    public class UserDto : IMapFrom<UserEntity>
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public string Token { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserEntity, UserDto>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(u => $"{u.FirstName} {u.LastName}"));
        }
    }
}
