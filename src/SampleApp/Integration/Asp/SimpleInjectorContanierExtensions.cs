using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Diagnostics;

namespace SampleApp.Integration.Asp
{
	public static class SimpleInjectorContanierExtensions
	{
		public static void AddSimpleInjectorControllersFeature2(this Container container, IServiceProvider serviceProvider)
		{
			if (container == null) throw new ArgumentNullException(nameof(container));
			if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
			var service = serviceProvider.GetService<ApplicationPartManager>();
			var controllerFeature = new ControllerFeature();
			service.PopulateFeature(controllerFeature);
			container.RegisterControllerTypes(controllerFeature.Controllers.Select(t => t.AsType()));
		}

		public static void RegisterMvcViewComponents2(this Container container, IServiceProvider serviceProvider)
		{
			IViewComponentDescriptorProvider service = serviceProvider.GetService<IViewComponentDescriptorProvider>();
			container.RegisterMvcViewComponents(service);
		}

		private static void RegisterControllerTypes(this Container container, IEnumerable<Type> types)
		{
			foreach (Type type in types.ToArray())
			{
				Registration concreteRegistration = CreateConcreteRegistration(container, type);
				if (ShouldSuppressDisposableTransientComponent(type))
					concreteRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Derived type doesn't override Dispose, so it can be safely ignored.");
				container.AddRegistration(type, concreteRegistration);
			}
		}

		private static void RegisterViewComponentTypes(this Container container, IEnumerable<Type> types)
		{
			foreach (Type type in types.ToArray())
				container.AddRegistration(type, CreateConcreteRegistration(container, type));
		}

		private static Registration CreateConcreteRegistration(Container container, Type concreteType)
		{
			return container.Options.LifestyleSelectionBehavior.SelectLifestyle(concreteType).CreateRegistration(concreteType, container);
		}

		private static bool ShouldSuppressDisposableTransientComponent(Type controllerType)
		{
			if (!TypeInheritsFromController(controllerType))
				return false;
			return GetProtectedDisposeMethod(controllerType).DeclaringType == typeof(Controller);
		}

		private static bool TypeInheritsFromController(Type controllerType)
		{
			return typeof(Controller).GetTypeInfo().IsAssignableFrom(controllerType);
		}

		private static MethodInfo GetProtectedDisposeMethod(Type controllerType)
		{
			foreach (MethodInfo method in controllerType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
			{
				if (!method.IsPrivate && !method.IsPublic && (method.ReturnType == typeof(void) && method.Name == "Dispose") && (method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(bool)))
					return method;
			}
			return (MethodInfo)null;
		}
	}
}