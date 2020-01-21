using System;

namespace OnlineRoulette.Domain.Common
{
    public class AuditableEntity<T>
    {
        public virtual T Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
