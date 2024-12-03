using System.Threading.Tasks;
using TangoBotApi.Services.DI;

namespace TangoBotApi.Services.Notification
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


