using OnlineRoulette.Domain.Common;
using OnlineRoulette.Domain.Enums;

namespace OnlineRoulette.Domain.Entities
{
    /// <summary>
    /// Spin generated when betting statrting
    /// </summary>
    public class SpinEntity : AuditableEntity<long>
    {
        /// <summary>
        /// Status failed, started, closed 
        /// </summary>
        public SpinStatus SpinStatus { get; set; }
        public int? WinningNumber { get; set; }
    }
}
