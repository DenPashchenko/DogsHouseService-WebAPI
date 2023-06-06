using DogsHouseService.Application.Properties;

namespace DogsHouseService.Application.Common.Exceptions
{
	public class TooManyRequestsException : Exception
	{
		public TooManyRequestsException()
			: base(string.Format(Resources.TooManyRequests)) { }
	}
}
