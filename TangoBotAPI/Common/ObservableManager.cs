using System;
using System.Collections.Generic;

namespace TangoBot.Core.Domain.Aggregates
{
    public class ObservableManager<T> : IObservable<T>
    {
        private readonly List<IObserver<T>> _observers;

        public ObservableManager()
        {
            _observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }

        public void Notify(T data)
        {
            foreach (var observer in _observers)
            {
                if (data == null)
                {
                    observer.OnError(new ArgumentNullException(nameof(data)));
                }
                else
                {
                    observer.OnNext(data);
                }
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in _observers.ToArray())
            {
                if (_observers.Contains(observer))
                {
                    observer.OnCompleted();
                }
            }
            _observers.Clear();
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<T>> _observers;
            private readonly IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer);
                }
            }
        }
    }
}
