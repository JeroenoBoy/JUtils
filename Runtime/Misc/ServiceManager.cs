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
        /// Check if a service exists
        /// </summary>
        public static bool Exists<T>() where T : IService
        {
            return instance._services.ContainsKey(typeof(T));
        }


        /// <summary>
        /// Check if a service exists
        /// </summary>
        public static bool Exists(Type serviceType)
        {
            return instance._services.ContainsKey(serviceType);
        }


        /// <summary>
        /// Get the instance of the given service
        /// </summary>
        public static T Get<T>() where T : IService
        {
            return (T)Get(typeof(T));
        }


        /// <summary>
        /// Get the instance of the given service
        /// </summary>
        public static IService Get(Type serviceType)
        {
            return instance._services[serviceType];
        }


        /// <summary>
        /// Register a new service and initialize it. Will throw if it already exists.
        /// </summary>
        public static void Register(Type serviceType, IService service)
        {
            if (!AssemblyJUtils.ExtendsClassOrInterface(service.GetType(), serviceType)) throw new InvalidOperationException("The service must extend the given type");
            instance._services.Add(serviceType, service);
            service.Init();
        }


        /// <summary>
        /// Remove an existing service and dispose it
        /// </summary>
        public static void Remove<T>() where T : IService
        {
            Remove(typeof(T));
        }


        /// <summary>
        /// Remove an existing service and dispose it
        /// </summary>
        public static void Remove(Type type)
        {
            IService service = Get(type);
            instance._services.Remove(type);
            service.Dispose();
        }
    }
}