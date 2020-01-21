namespace OnlineRoulette.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int CurrentUserId { get; }
        string IpAddress { get; }
    }
}
