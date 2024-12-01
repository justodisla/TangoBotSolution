using System.Threading.Tasks;

namespace TangoBotApi.Infrastructure
{
    /// <summary>
    /// Provides notification functionalities.
    /// </summary>
    public interface INotificationService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendSmsAsync(string to, string message);
    }
}


