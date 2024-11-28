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

            _streamingService = TangoBotServiceProviderExp.GetSingletonService<IStreamingService>();

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

    }
}
