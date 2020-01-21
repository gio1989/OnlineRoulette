namespace OnlineRoulette.Domain.Enums
{
    public enum SpinStatus : byte
    {
        /// <summary>
        /// when betting started
        /// </summary>
        started = 1,

        /// <summary>
        /// when betting closed
        /// </summary>
        Closed = 2,

        /// <summary>
        /// something wrong happened, for ex: network connection fail.
        /// </summary>
        Failed = 3
    }
}
