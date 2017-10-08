using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp.Application.Home
{
	public class HomeController : Controller
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IMsScopeCounter _msScopeCounter;
		private readonly IMsSingletonCounter _msSingletonCounter;
		private readonly ISimpleScopeCounter _simpleScopeCounter;
		private readonly ISimpleSingletonCounter _simpleSingletonCounter;

		public HomeController(IHostingEnvironment hostingEnvironment,
			IMsScopeCounter msScopeCounter,
			IMsSingletonCounter msSingletonCounter,
			ISimpleScopeCounter simpleScopeCounter,
			ISimpleSingletonCounter simpleSingletonCounter)
		{
			if(hostingEnvironment == null)
				throw new Exception("Cross wiring has failed with the composite service locator");

			_hostingEnvironment = hostingEnvironment;
			_msScopeCounter = msScopeCounter;
			_msSingletonCounter = msSingletonCounter;
			_simpleScopeCounter = simpleScopeCounter;
			_simpleSingletonCounter = simpleSingletonCounter;
		}

		[Route("")]
		public IActionResult Home()
		{
			return View("Home/Home", new CounterModel
			{
				MsScopedCounter = _msScopeCounter.InstancesCreated,
				MsSingletonCounter = _msSingletonCounter.InstancesCreated,
				SimpleScopedCounter = _simpleScopeCounter.InstancesCreated,
				SimpleSingletonCounter = _simpleSingletonCounter.InstancesCreated
			});
		}
	}

	public class CounterModel
	{
		public int MsScopedCounter { get; set; }
		public int MsSingletonCounter { get; set; }

		public int SimpleScopedCounter { get; set; }
		public int SimpleSingletonCounter { get; set; }
	}
}