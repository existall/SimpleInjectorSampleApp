using System;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Application.Home
{
    public class HomeController : Controller
    {
	    private readonly IHelloProvider _helloProvider;

	    public HomeController(IHelloProvider helloProvider)
	    {
		    if (helloProvider == null) throw new ArgumentNullException(nameof(helloProvider));
		    _helloProvider = helloProvider;
	    }

	    [Route("")]
	    public IActionResult Home()
		{
			return View("Home/Home");
		}
    }
}
