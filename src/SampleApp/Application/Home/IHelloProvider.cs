using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApp.Application.Home
{
	public interface IHelloProvider
	{
		int SayHello();
	}

	class HelloProvider : IHelloProvider
	{
		private static int _counter;

		public HelloProvider()
		{
			_counter++;
		}

		public int SayHello()
		{
			return _counter;
		}
	}
}
