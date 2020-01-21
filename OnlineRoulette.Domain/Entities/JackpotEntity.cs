using OnlineRoulette.Domain.Common;

namespace OnlineRoulette.Domain.Entities
{
    public class JackpotEntity : AuditableEntity<int>
    {
        public decimal Amount { get; set; }
        public bool IsActive { get; set; }
    }
}
