using System.Text;
using System.Threading.Tasks;
using TangoBotApi.Services.Http;
using TangoBotApi.Services.Notification;

namespace TangoBotApi.Infrastructure
{

    public class NotificationService : INotificationService
    {
        private IHttpClient _httpClient;

        private bool _initialized = false;

        public NotificationService()
        {
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _httpClient = ServiceLocator.GetSingletonService<IHttpClient>();

            _initialized = true;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            Initialize();

            // Replace with actual email sending logic
            var emailApiUrl = "https://api.example.com/sendEmail";
            var emailContent = new StringContent(
                $"{{\"to\":\"{to}\",\"subject\":\"{subject}\",\"body\":\"{body}\"}}",
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(emailApiUrl, emailContent);
            response.EnsureSuccessStatusCode();
        }

        public async Task SendSmsAsync(string to, string message)
        {
            Initialize();

            // Replace with actual SMS sending logic
            var smsApiUrl = "https://api.example.com/sendSms";
            var smsContent = new StringContent(
                $"{{\"to\":\"{to}\",\"message\":\"{message}\"}}",
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(smsApiUrl, smsContent);
            response.EnsureSuccessStatusCode();
        }

        public string[] Requires()
        {
            //throw new System.NotImplementedException();
            return new[] { typeof(IHttpClient).FullName! };
        }

        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
}
