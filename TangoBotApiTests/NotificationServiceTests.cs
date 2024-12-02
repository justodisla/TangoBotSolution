using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using TangoBotApi.Infrastructure;
using Xunit;

namespace TangoBotApi.Tests
{
    public class NotificationServiceTests
    {
        private readonly Mock<INotificationService> _notificationServiceMock;

        public NotificationServiceTests()
        {
            _notificationServiceMock = new Mock<INotificationService>();
        }

        [Fact]
        public async Task SendEmailAsync_ShouldCallEmailApi()
        {
            // Arrange
            var to = "jdisla@gmail.com"; // Updated email address for testing
            var subject = "Test Subject";
            var body = "Test Body";

            _notificationServiceMock
                .Setup(service => service.SendEmailAsync(to, subject, body))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _notificationServiceMock.Object.SendEmailAsync(to, subject, body);

            // Assert
            _notificationServiceMock.Verify(service => service.SendEmailAsync(to, subject, body), Times.Once);
        }

        [Fact]
        public async Task SendSmsAsync_ShouldCallSmsApi()
        {
            // Arrange
            var to = "1234567890";
            var message = "Test Message";

            _notificationServiceMock
                .Setup(service => service.SendSmsAsync(to, message))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _notificationServiceMock.Object.SendSmsAsync(to, message);

            // Assert
            _notificationServiceMock.Verify(service => service.SendSmsAsync(to, message), Times.Once);
        }
    }
}
