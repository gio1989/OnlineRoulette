using OnlineRoulette.Domain.Common;
using OnlineRoulette.Domain.Enums;

namespace OnlineRoulette.Domain.Entities
{
    public class BetEntity : AuditableEntity<int>
    {
        public int UserId { get; set; }
        public decimal BetAmount { get; set; }
        public int SpinId { get; set; }
        public string BetString { get; set; }
        public decimal WonAmount { get; set; }
        public string IpAddress { get; set; }
        public BetStatus BetStatus { get; set; }
        public decimal? JackpotAmount { get; set; }
        public UserEntity User { get; set; }
        public SpinEntity Spin { get; set; }
    }
}
