using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;

namespace SampleApp.Integration.Asp
{
	internal class SimpleInjectorServiceProviderFactory : IServiceProviderFactory<Container>
	{
		private Container Container { get; set; }
		private IServiceCollection Services { get; set; }

		public Container CreateBuilder(IServiceCollection services)
		{
			Container = Container ?? new Container();
			Services = services;
			return Container;
		}

		public IServiceProvider CreateServiceProvider(Container container)
		{
			Services.UseSimpleInjectorAspNetRequestScoping(container);
			Services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
			Services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));
			var serviceProvider = Services.BuildServiceProvider();
			container.AddSimpleInjectorControllersFeature2(serviceProvider);
			container.RegisterMvcViewComponents2(serviceProvider);
			var simpleInjectorBridgeServiceProvider = new SimpleInjectorBridgeServiceProvider(serviceProvider, Container);
			container.RegisterSingleton<IServiceScopeFactory>(new SimpleInjectorBridgeServiceScopeFactory(simpleInjectorBridgeServiceProvider, serviceProvider.GetService<IServiceScopeFactory>()));
			container.RegisterSingleton<IServiceProvider>(simpleInjectorBridgeServiceProvider);
			return simpleInjectorBridgeServiceProvider;
		}
	}
}