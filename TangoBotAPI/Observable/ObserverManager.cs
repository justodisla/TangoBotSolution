using System;
using System.Collections.Generic;

/// <summary>
/// Manages a list of observers and notifies them of events.
/// Implements the IObservable interface.
/// </summary>
/// <typeparam name="T">The type of event data.</typeparam>
public class ObserverManager<T> : IObservable<T>
{
    private readonly List<IObserver<T>> _observers;

    /// <summary>
    /// Initializes a new instance of the ObserverManager class.
    /// </summary>
    public ObserverManager()
    {
        _observers = new List<IObserver<T>>();
    }

    /// <summary>
    /// Subscribes an observer to the observable.
    /// </summary>
    /// <param name="observer">The observer to subscribe.</param>
    /// <returns>A disposable object that can be used to unsubscribe the observer.</returns>
    public IDisposable Subscribe(IObserver<T> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    /// <summary>
    /// Notifies all subscribed observers of an event.
    /// </summary>
    /// <param name="eventData">The event data to notify observers with.</param>
    public void Notify(T eventData)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(eventData);
        }
    }

    /// <summary>
    /// Manages the unsubscription of observers.
    /// </summary>
    private class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        /// <summary>
        /// Initializes a new instance of the Unsubscriber class.
        /// </summary>
        /// <param name="observers">The list of observers.</param>
        /// <param name="observer">The observer to unsubscribe.</param>
        public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        /// <summary>
        /// Unsubscribes the observer from the list of observers.
        /// </summary>
        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}
