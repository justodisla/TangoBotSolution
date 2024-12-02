using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.EventBus;

namespace TangoBotApi.Infrastructure
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<string, List<IEventListener>> _eventHandlers = new();

        public void Subscribe(string eventName, IEventListener listener)
        {
            if (!_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName] = new List<IEventListener>();
            }
            _eventHandlers[eventName].Add(listener);
        }

        public void Unsubscribe(string eventName, IEventListener listener)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                _eventHandlers[eventName].Remove(listener);
            }
        }

        public async Task PublishAsync(string eventName, object eventData)
        {
            if (_eventHandlers.ContainsKey(eventName))
            {
                foreach (var handler in _eventHandlers[eventName])
                {
                     handler.HandleEventAsync(eventData);
                }
            }
        }
    }
}
