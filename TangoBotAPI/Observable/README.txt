the Observable Pattern Variation
This guide provides instructions on how to implement and use the ObserverManager<T> class to create an observable pattern variation in your .NET 6 projects.
Overview
The ObserverManager<T> class is a reusable helper that manages a list of observers and notifies them of events. This class can be integrated into any service that needs to implement the observable pattern.
Steps to Implement and Use
1. Define the Event Data
Create a class to represent the event data.

public class MarketEvent
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}

2. Implement the Service
Integrate the ObserverManager into your service class.

using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class MarketEventService : IObservable<MarketEvent>
{
    private readonly ObserverManager<MarketEvent> _observerManager;
    private readonly ClientWebSocket _webSocket;

    public MarketEventService()
    {
        _observerManager = new ObserverManager<MarketEvent>();
        _webSocket = new ClientWebSocket();
    }

    public IDisposable Subscribe(IObserver<MarketEvent> observer)
    {
        return _observerManager.Subscribe(observer);
    }

    public async Task StartListeningAsync(string uri)
    {
        await _webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);

        var buffer = new byte[1024 * 4];
        while (_webSocket.State == WebSocketState.Open)
        {
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var marketEvent = JsonConvert.DeserializeObject<MarketEvent>(message);
                _observerManager.Notify(marketEvent);
            }
        }
    }
}

3. Implement the Observer
Create a class that implements IObserver<T> to handle the events.

using System;

public class MarketEventSubscriber : IObserver<MarketEvent>
{
    public void OnCompleted()
    {
        // Handle completion
    }

    public void OnError(Exception error)
    {
        // Handle error
    }

    public void OnNext(MarketEvent value)
    {
        Console.WriteLine($"Market event received: {value.Symbol} - {value.Price} at {value.Timestamp}");
    }
}

4. Usage
Subscribe to the observable and start the service.

class Program
{
    static async Task Main(string[] args)
    {
        var service = new MarketEventService();
        var subscriber = new MarketEventSubscriber();

        using (service.Subscribe(subscriber))
        {
            await service.StartListeningAsync("wss://example.com/marketdata");
        }

        // After disposing, no more notifications will be received
    }
}

Explanation
•	MarketEvent: Represents the event data.
•	ObserverManager: Manages a list of observers and notifies them of events. It implements IObservable<T>.
•	MarketEventService: Integrates the ObserverManager to handle subscriptions and notifications. It listens for events via WebSocket and notifies subscribers.
•	MarketEventSubscriber: Implements IObserver<T> to handle the events.
This implementation ensures that every time an event is received via WebSocket, all subscribed observers are notified. The ObserverManager class can be reused for other services, making the code more modular and maintainable.
Summary
By following this guide, you can implement the observable pattern variation using the ObserverManager<T> class in your .NET 6 projects. This approach helps in managing observers efficiently and ensures that your services can notify subscribers of events seamlessly.

