namespace DogsHouseService.WebApi.Middleware.RateLimiting
{
	public class ClientStatistics
	{
		public DateTime LastSuccessfulResponseTime { get; set; }
		public int NumberOfRequestsCompletedSuccessfully { get; set; }
	}
}
