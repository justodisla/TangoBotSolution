using System.Threading.Tasks;
using TangoBotApi.DI;

namespace TangoBotApi.Infrastructure
{
    /// <summary>
    /// Provides notification functionalities.
    /// </summary>
    public interface INotificationService : IInfrService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendSmsAsync(string to, string message);
    }
}


