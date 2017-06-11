using System;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApp.Integration.Asp
{
	internal sealed class SimpleInjectorBridgeServiceScopeFactory : IServiceScopeFactory
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public SimpleInjectorBridgeServiceScopeFactory(IServiceProvider serviceProvider, IServiceScopeFactory serviceScopeFactory)
		{
			_serviceProvider = serviceProvider;
			_serviceScopeFactory = serviceScopeFactory;
		}

		public IServiceScope CreateScope()
		{
			return new StructureMapServiceScope(_serviceProvider, _serviceScopeFactory.CreateScope());
		}

		private class StructureMapServiceScope : IServiceScope
		{
			private readonly IServiceScope _serviceScope;

			public StructureMapServiceScope(IServiceProvider serviceProvider, IServiceScope serviceScope)
			{
				ServiceProvider = serviceProvider;
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