using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Cli;

namespace Husk
{
    public class ServiceRegistrar : ITypeRegistrar
    {
        public static IServiceCollection BuildServiceCollection() {
            return new ServiceCollection();
        }
        internal readonly IServiceCollection ServiceCollection;

        public ServiceRegistrar(IServiceCollection services)
        {
            ServiceCollection = services;
        }
        public ITypeResolver Build()
        {
            return new ServiceResolver(ServiceCollection.BuildServiceProvider());
        }

        public void Register(Type service, Type implementation)
        {
            ServiceCollection.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            ServiceCollection.AddSingleton(service, implementation);
        }

        public class ServiceResolver : ITypeResolver
        {
            private readonly ServiceProvider _provider;

            public ServiceResolver(ServiceProvider provider)
            {
                _provider = provider;
            }
            public object Resolve(Type type)
            {
                return _provider.GetService(type);
            }
        }
    }
}