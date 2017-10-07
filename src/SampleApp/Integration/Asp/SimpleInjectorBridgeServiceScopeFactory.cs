using System;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace SampleApp.Integration.Asp
{
	internal sealed class SimpleInjectorBridgeServiceScopeFactory : IServiceScopeFactory
	{
		private readonly Container _container;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public SimpleInjectorBridgeServiceScopeFactory(Container container, IServiceScopeFactory serviceScopeFactory)
		{
			_container = container;
			_serviceScopeFactory = serviceScopeFactory;
		}

		public IServiceScope CreateScope()
		{
			return new SimpleInjectorBridgeServiceScope(_container, _serviceScopeFactory.CreateScope());
		}

		private class SimpleInjectorBridgeServiceScope : IServiceScope
		{
			private readonly IServiceScope _serviceScope;

			public SimpleInjectorBridgeServiceScope(Container container, IServiceScope serviceScope)
			{
				ServiceProvider = new SimpleInjectorBridgeServiceProvider(serviceScope.ServiceProvider, container);
				_serviceScope = serviceScope;
			}

			public IServiceProvider ServiceProvider { get; }

			public void Dispose()
			{
				_serviceScope.Dispose();
			}
		}
	}
}