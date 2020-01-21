namespace OnlineRoulette.Application.AuthManager
{
    public interface IAuthManager
    {
        bool IsEmailValid(string mail);
        string ComputeHash(string input);
        string GenerateSalt(int size);
        string GenerateTemporaryPassword(int length);
        string GenerateToken(int userId);
    }
}
