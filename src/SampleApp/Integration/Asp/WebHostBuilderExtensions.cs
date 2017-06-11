using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace SampleApp.Integration.Asp
{
    public static class WebHostBuilderExtensions
    {
	    public static IWebHostBuilder IntegrateSimpleInjectorWithMvc(this IWebHostBuilder target)
	    {
		    target.ConfigureServices(x => x.AddSingleton<IServiceProviderFactory<Container>>(new SimpleInjectorServiceProviderFactory()));
		    return target;
	    }
	}
}
