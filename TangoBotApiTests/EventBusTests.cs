using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangoBot.Infrastructure.DependencyInjection;
using TangoBotApi.EventBus;
using TangoBotApi.Infrastructure;
using Xunit;

namespace TangoBotApi.Tests
{
    public class EventBusTests
    {
        private readonly IEventBus _eventBus;

        public EventBusTests()
        {
            ServiceLocator.Initialize();
            _eventBus = ServiceLocator.GetSingletonService<IEventBus>() as IEventBus;
        }

        [Fact]
        public void Subscribe_ShouldAddListener()
        {
            // Arrange
            var eventName = "TestEvent";
            var listener = new TestEventListener();

            // Act
            _eventBus.Subscribe(eventName, listener);

            // Assert
            var listeners = _eventBus.GetListeners(eventName);
            Assert.Contains(listener, listeners);
        }

        [Fact]
        public void Unsubscribe_ShouldRemoveListener()
        {
            // Arrange
            var eventName = "TestEvent";
            var listener = new TestEventListener();
            _eventBus.Subscribe(eventName, listener);

            // Act
            _eventBus.Unsubscribe(eventName, listener);

            // Assert
            var listeners = _eventBus.GetListeners(eventName);
            Assert.DoesNotContain(listener, listeners);
        }

        [Fact]
        public async Task PublishAsync_ShouldNotifySubscribedListeners()
        {
            // Arrange
            var eventName = "TestEvent";
            var eventData = new { Message = "Test Message" };
            var listener = new TestEventListener();
            _eventBus.Subscribe(eventName, listener);

            // Act
            await _eventBus.PublishAsync(eventName, eventData);

            // Assert
            Assert.True(listener.EventHandled);
            Assert.Equal(eventData, listener.ReceivedEventData);
        }

        

        private class TestEventListener : IEventListener
        {
            public bool EventHandled { get; private set; }
            public object ReceivedEventData { get; private set; }

            public Task HandleEventAsync(object eventData)
            {
                EventHandled = true;
                ReceivedEventData = eventData;
                return Task.CompletedTask;
            }
        }
    }
}
