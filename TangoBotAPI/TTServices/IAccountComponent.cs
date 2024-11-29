namespace TangoBot.API.TTServices
{
    public interface IAccountComponent
    {
        Task<Dictionary<string, object>> GetAccountBalancesAsync(string accountNumber);
        Task<Dictionary<string, object>[]> GetAccountPositionsAsync(string accountNumber);
        Task<Dictionary<string, object>[]?> GetBalanceSnapshotAsync(string accountNumber);
    }
}