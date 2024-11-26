using HttpClientLib.OrderApi.Models;

namespace HttpClientLib.OrderApi
{
    public interface IOrderComponent<T>
    {
        Task<T> CancelOrderByIdAsync(string accountNumber, int orderId);
        Task<T[]?> GetAccountLiveOrdersAsync(string accountNumber);
        Task<T[]?> GetAccountOrdersAsync(string accountNumber);
        Task<T> GetOrderByIdAsync(string accountNumber, int orderId);
        Task<OrderPostReport> PostEquityOrder(string accountNumber, OrderRequest orderRequest, bool isDryRun = true);
    }
}