namespace SampleApp.Application.Home
{
	public class MsScopeCounter : IMsScopeCounter
	{
		private static int _counter;

		public MsScopeCounter()
		{
			_counter++;
		}

		public int InstancesCreated => _counter;
	}

	public class SimpleScopeCounter : ISimpleScopeCounter
	{
		private static int _counter;

		public SimpleScopeCounter()
		{
			_counter++;
		}

		public int InstancesCreated => _counter;
	}
}