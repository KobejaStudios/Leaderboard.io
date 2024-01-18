using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Leaderboard.io
{
    public class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void RegisterService<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public static T GetService<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"Service of type {typeof(T)} not registered.");
        }
    }
}
