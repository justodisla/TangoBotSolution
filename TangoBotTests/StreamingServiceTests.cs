using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TangoBotAPI.Streaming;
using TangoBotAPI.DI;
using TangoBot;

namespace TangoBotStreaming.Tests
{
    public class StreamingServiceTests
    {
        //private readonly Mock<ClientWebSocket> _mockWebSocketClient;
        private readonly Mock<ObserverManager<HistoricDataReceivedEvent>> _mockObserverManager;
        private readonly TangoBotAPI.Streaming.IStreamingService _streamingService;

        public StreamingServiceTests()
        {
            //_mockWebSocketClient = new Mock<ClientWebSocket>();
            //_mockObserverManager = new Mock<ObserverManager<CandleEvent>>();
            //_streamingService = new StreamingService(
            //    "wss://example.com/socket",
            //    _mockWebSocketClient.Object,
            //    _mockObserverManager.Object
            //);

            StartUp.InitializeDI();

            _streamingService = TangoBotServiceProvider.GetSingletonService<IStreamingService>();

        }

        [Fact]
        public async Task StreamHistoricDataAsync_ShouldReturnQuoteDataHistory()
        {
            // Arrange
            var symbol = "AAPL";
            var fromTime = DateTime.UtcNow.AddDays(-10);
            var toTime = DateTime.UtcNow;
            var timeframe = Timeframe.Daily;
            var interval = 1;

            // Act
            _streamingService.StreamHistoricDataAsync(symbol, fromTime, toTime, timeframe, interval);

            // Assert
            //Assert.NotNull(result);
            //Assert.IsType<QuoteDataHistory>(result);
        }

        [Fact(Skip = "Later")]
        public void PatchHistoricData_ShouldPatchData()
        {
            // Arrange
            var quoteDataHistory = new QuoteDataHistory();

            // Act
            _streamingService.PatchHistoricData(quoteDataHistory);

            // Assert
            // Add assertions to verify the behavior
        }

        [Fact(Skip = "Later")]
        public void StreamLiveMarketData_ShouldStreamData()
        {
            // Arrange
            var quoteDataHistory = new QuoteDataHistory();

            // Act
            _streamingService.StreamLiveMarketData(quoteDataHistory);

            // Assert
            // Add assertions to verify the behavior
        }

        [Fact(Skip = "Later")]
        public void CloseWsConnection_ShouldCloseConnection()
        {
            // Act
            _streamingService.CloseWsConnection();

            // Assert
            //_mockWebSocketClient.Verify(ws => ws.CloseAsync(It.IsAny<WebSocketCloseStatus>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(Skip = "Later")]
        public async Task IsStreamingAuthTokenValid_ShouldReturnTrueForValidToken()
        {
            // Arrange
            var validToken = "valid_token";

            // Act
            var result = await _streamingService.IsStreamingAuthTokenValid(validToken);

            // Assert
            Assert.True(result);
        }

        [Fact(Skip = "Later")]
        public void Subscribe_ShouldReturnDisposable()
        {
            // Arrange
            var observer = new Mock<IObserver<HistoricDataReceivedEvent>>();

            // Act
            var disposable = "";// _streamingService.Subscribe(observer.Object);

            // Assert
            Assert.NotNull(disposable);
        }
    }
}
