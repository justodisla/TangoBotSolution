using System;
using System.Collections.Generic;

public class DependencyContainer
{
    private readonly Dictionary<Type, object> _registrations = new();

    // Register a singleton instance of a service
    public void Register<TInterface, TImplementation>() where TImplementation : class, TInterface, new()
    {
        _registrations[typeof(TInterface)] = new TImplementation();
    }

    // Register an existing instance of a service
    public void Register<TInterface>(TInterface instance)
    {
        _registrations[typeof(TInterface)] = instance;
    }

    // Resolve a service by its interface
    public T Resolve<T>()
    {
        if (_registrations.TryGetValue(typeof(T), out var instance))
        {
            return (T)instance;
        }
        throw new InvalidOperationException($"Service of type {typeof(T)} is not registered.");
    }
}