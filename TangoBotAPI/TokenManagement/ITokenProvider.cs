namespace TangoBotAPI.TokenManagement
{
    public interface ITokenProvider
    {
        Task<string?> GetValidTokenAsync();
    }
}