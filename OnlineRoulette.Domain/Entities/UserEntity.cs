
using OnlineRoulette.Domain.Common;
using System;

namespace OnlineRoulette.Domain.Entities
{
    public class UserEntity : AuditableEntity<int>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
