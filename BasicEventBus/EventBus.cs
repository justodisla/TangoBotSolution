using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TangoBotApi.EventBus;

namespace TangoBot.Infrastructure.BasicEventBus
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, List<IEventListener>> _eventHandlers = new();
        private readonly ReaderWriterLockSlim _lock = new();

        public void Subscribe(string eventName, IEventListener listener)
        {
            _lock.EnterWriteLock();
            try
            {
                if (!_eventHandlers.ContainsKey(eventName))
                {
                    _eventHandlers[eventName] = new List<IEventListener>();
                }
                _eventHandlers[eventName].Add(listener);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Unsubscribe(string eventName, IEventListener listener)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_eventHandlers.ContainsKey(eventName))
                {
                    _eventHandlers[eventName].Remove(listener);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public async Task PublishAsync(string eventName, object eventData)
        {
            List<IEventListener> handlers;
            _lock.EnterReadLock();
            try
            {
                if (!_eventHandlers.ContainsKey(eventName))
                {
                    return;
                }
                handlers = new List<IEventListener>(_eventHandlers[eventName]);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            foreach (var handler in handlers)
            {
                await handler.HandleEventAsync(eventData);
            }
        }

        public IList<IEventListener> GetListeners(string eventName)
        {
            _lock.EnterReadLock();
            try
            {
                if (_eventHandlers.ContainsKey(eventName))
                {
                    return new List<IEventListener>(_eventHandlers[eventName]);
                }
                return new List<IEventListener>();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
