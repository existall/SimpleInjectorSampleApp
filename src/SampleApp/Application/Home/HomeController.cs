using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Application.Home
{
    public class HomeController : Controller
    {
	    private readonly IHelloProvider _helloProvider;
	    private readonly IHostingEnvironment _hostingEnvironment;

	    public HomeController(IHelloProvider helloProvider,
			IHostingEnvironment hostingEnvironment)
	    {
		    _helloProvider = helloProvider;
		    _hostingEnvironment = hostingEnvironment;
	    }

	    [Route("")]
	    public IActionResult Home()
	    {
		    var sayHello = _helloProvider.SayHello();

		    return View("Home/Home", sayHello);
		}
    }
}

public interface ISomeProvider1
{
	string Get();
}

public class SomeProvider1 : ISomeProvider1
{
	private static int _counter;

	public SomeProvider1()
	{
		_counter++;
	}

	public string Get()
	{
		return _counter.ToString();
	}
}

public interface ISomeProvider
{
	string Get();
}

public class SomeProvider : ISomeProvider
{
	private static int _counter;

	public SomeProvider()
	{
		_counter++;
	}

	public string Get()
	{
		return _counter.ToString();
	}
}