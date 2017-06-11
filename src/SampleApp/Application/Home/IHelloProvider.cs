using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Application.Home
{
	public interface IHelloProvider
	{
		string SayHello();
	}

	class HelloProvider : IHelloProvider
	{
		public string SayHello()
		{
			return "Hello";
		}
	}
}
