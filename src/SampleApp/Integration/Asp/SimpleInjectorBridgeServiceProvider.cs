using System;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace SampleApp.Integration.Asp
{
	internal class SimpleInjectorBridgeServiceProvider : IServiceProvider, ISupportRequiredService
	{
		private readonly IServiceProvider _provider;
		private readonly Container _container;

		public SimpleInjectorBridgeServiceProvider(IServiceProvider serviceProvider, Container container)
		{
			_provider = serviceProvider;
			_container = container;
		}

		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(IServiceScopeFactory))
			{
				return _container.GetInstance(serviceType);
			}

			var instance = _provider.GetService(serviceType);
			return instance ?? _container.GetInstance(serviceType);
		}

		public object GetRequiredService(Type serviceType)
		{
			if (serviceType == typeof(IServiceScopeFactory))
			{
				return _container.GetInstance(serviceType);
			}

			var instance = _provider.GetService(serviceType);
			return instance ?? _container.GetRequiredService(serviceType);
		}
	}
}