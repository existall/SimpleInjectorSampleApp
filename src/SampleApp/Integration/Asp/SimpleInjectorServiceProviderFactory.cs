using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using SimpleInjector.Integration.AspNetCore;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;

[assembly: AspMvcViewLocationFormat(@"~\Application\{0}.cshtml")]
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
			 Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
			return Container;
		}

		public IServiceProvider CreateServiceProvider(Container container)
		{
			Services.UseSimpleInjectorAspNetRequestScoping(container);

			Services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));

            Services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));

			Services.AddScoped<ISomeProvider, SomeProvider>();

			Services.AddSingleton<ISomeProvider1, SomeProvider1>();

			var serviceProvider = Services.BuildServiceProvider();

            container.AddSimpleInjectorControllersFeature2(serviceProvider);

            container.RegisterMvcViewComponents2(serviceProvider);

            var simpleInjectorBridgeServiceProvider = new SimpleInjectorBridgeServiceProvider(serviceProvider, Container);

            container.RegisterSingleton<IServiceScopeFactory>(new SimpleInjectorBridgeServiceScopeFactory(container, serviceProvider.GetService<IServiceScopeFactory>()));

            container.RegisterSingleton<IServiceProvider>(simpleInjectorBridgeServiceProvider);

			container.ConfigureAutoCrossWiring(serviceProvider, Services);


			return simpleInjectorBridgeServiceProvider;
		}
	}

	public static class X
	{
		public static void ConfigureAutoCrossWiring(
	this Container container, IServiceProvider builder, IServiceCollection services)
		{
			container.ResolveUnregisteredType += (s, e) =>
			{
				if (e.Handled) return;

				Type serviceType = e.UnregisteredServiceType;

				ServiceDescriptor descriptor = services.LastOrDefault(d => d.ServiceType == serviceType);

				if (descriptor == null && serviceType.GetTypeInfo().IsGenericType)
				{
					var serviceTypeDefinition = serviceType.GetTypeInfo().GetGenericTypeDefinition();
					descriptor = services.LastOrDefault(d => d.ServiceType == serviceTypeDefinition);
				}

				if (descriptor != null)
				{
					Lifestyle lifestyle =
						descriptor.Lifetime == ServiceLifetime.Singleton ? Lifestyle.Singleton :
						descriptor.Lifetime == ServiceLifetime.Scoped ? Lifestyle.Scoped :
						Lifestyle.Transient;

					Registration registration = lifestyle.CreateRegistration(serviceType, () =>
					{
						var provider = builder.GetService<IHttpContextAccessor>().HttpContext.RequestServices.GetService(serviceType);
						return provider;
					},
						container);

					if (lifestyle == Lifestyle.Transient && typeof(IDisposable).IsAssignableFrom(serviceType))
					{
						registration.SuppressDiagnosticWarning(
							DiagnosticType.DisposableTransientComponent,
							justification:
								"This is a cross-wired service. ASP.NET will ensure it gets disposed.");
					}

					e.Register(registration);
				}
			};
		}
	}


}