using System.IO;
using Microsoft.AspNetCore.Hosting;
using SampleApp.Integration.Asp;
using SimpleInjector.Lifestyles;

namespace SampleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseIISIntegration()
				.UseSimpleInjector(o => o.DefaultScopedLifestyle = new AsyncScopedLifestyle())
				.UseStartup<Startup>()
				.UseApplicationInsights()
				.Build();

			host.Run();
		}
	}
}
