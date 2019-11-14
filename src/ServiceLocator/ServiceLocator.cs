﻿using ServiceLocator.Exeptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceLocator
{
    public interface IServiceLocator
    {
        IServiceLocator Register<TInterface, TImplementation>()
            where TImplementation : TInterface;

        IServiceLocator Register(Type interfaceType, Type implementationType);

        IServiceLocator RegisterAsSingleton<TInterface, TImplementation>()
            where TImplementation : TInterface;

        IServiceLocator RegisterAsSingleton(Type interfaceType, Type implementationType);

        TImplementation Resolve<TImplementation>();

        object Resolve(Type type);
    }

    public class ServiceLocator : IServiceLocator
    {
        private readonly IDictionary<Type, object> _singletons = new Dictionary<Type, object>();
        private readonly IDictionary<Type, Type> _implementations = new Dictionary<Type, Type>();
        private static IServiceLocator _instance;
        private ServiceLocator()
        {

        }

        public static IServiceLocator Create()
        {
            if (_instance == null)
            {
                _instance = new ServiceLocator();
            }

            return _instance;
        }

        public IServiceLocator Register<TInterface, TImplementation>() where TImplementation : TInterface
         => Register(typeof(TInterface), typeof(TImplementation));

        public IServiceLocator Register(Type interfaceType, Type implementationType)
        {
            if(_implementations.ContainsKey(interfaceType))
            {
                _implementations.Remove(interfaceType);
            }

            _implementations.Add(interfaceType, implementationType);

            return this;
        }

        public TImplementation Resolve<TImplementation>()
            => (TImplementation)Resolve(typeof(TImplementation));

        public object Resolve(Type type)
        {
            if(_singletons.TryGetValue(type, out var singleton) && singleton != null)
            {
                return singleton;
            }

            if(!_implementations.TryGetValue(type, out var impType))
            {
                throw new ServiceLocatorExeptions($"{type.Name} Implementation missing");
            }

            var constructors = impType.GetConstructors().FirstOrDefault();
            var parameters = constructors.GetParameters();
            var implementations = new List<object>();

            foreach(var paramiter in parameters)
            {
                var parameterInstance = Resolve(paramiter.ParameterType);
                implementations.Add(parameterInstance);
            }

            var instance = Activator.CreateInstance(impType, implementations.ToArray());

            if(_singletons.ContainsKey(type) && singleton == null)
            {
                _singletons[type] = instance;
            }

            return instance;
        }

        public IServiceLocator RegisterAsSingleton<TInterface, TImplementation>() where TImplementation : TInterface
            => RegisterAsSingleton(typeof(TInterface), typeof(TImplementation));

        public IServiceLocator RegisterAsSingleton(Type interfaceType, Type implementationType)
        {
            if (_singletons.ContainsKey(interfaceType))
            {
                _singletons.Remove(interfaceType);
            }

            _singletons.Add(interfaceType, null);

            Register(interfaceType, implementationType);
            return this;
        }
    }
}
