﻿using System;
using System.Collections.Generic;

namespace TangoBot.Core.App
{
    public partial class Application
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service instance with the application.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="service">The service instance.</param>
        public void RegisterService<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        /// <summary>
        /// Gets a service instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetService<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
        }
    }
}

