using System;
using System.Collections.Generic;

namespace JUtils
{
    public interface IService
    {
        public void Init();
        public void Dispose();
    }


    /// <summary>
    /// A simple service manager
    /// </summary>
    public sealed class ServiceManager : AutoSingletonBehaviour<ServiceManager>
    {
        private readonly Dictionary<Type, IService> _services = new();


        /// <summary>
        /// Get the instance of the given service
        /// </summary>
        public static T GetService<T>() where T : class, IService
        {
            return GetService(typeof(T)) as T;
        }


        /// <summary>
        /// Get the instance of the given service
        /// </summary>
        public static IService GetService(Type type)
        {
            return instance._services[type];
        }


        /// <summary>
        /// Register a new service and initialize it. Will throw if it already exists.
        /// </summary>
        public static void Register(Type serviceType, IService service)
        {
            instance._services.Add(serviceType, service);
            service.Init();
        }


        /// <summary>
        /// Remove an existing service and dispose it
        /// </summary>
        public static void RemoveService<T>() where T : IService
        {
            RemoveService(typeof(T));
        }


        /// <summary>
        /// Remove an existing service and dispose it
        /// </summary>
        public static void RemoveService(Type type)
        {
            IService service = GetService(type);
            instance._services.Remove(type);
            service.Dispose();
        }
    }
}