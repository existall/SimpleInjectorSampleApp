namespace SampleApp.Application.Home
{
	public class MsSingletonCounter : IMsSingletonCounter
	{
		private static int _counter;

		public MsSingletonCounter()
		{
			_counter++;
		}

		public int InstancesCreated => _counter;
	}

	public class SimpleSingletonCounter : ISimpleSingletonCounter
	{
		private static int _counter;

		public SimpleSingletonCounter()
		{
			_counter++;
		}

		public int InstancesCreated => _counter;
	}
}