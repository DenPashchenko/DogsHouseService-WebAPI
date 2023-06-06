using DogsHouseService.Application.Common.Exceptions;
using DogsHouseService.WebApi.Middleware.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace DogsHouseService.WebApi.Middleware.RateLimiting
{
	public class RateLimitingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IDistributedCache _cache;
		private readonly int _maxRequests;
		private readonly int _timeWindow;

		public RateLimitingMiddleware(RequestDelegate next, IDistributedCache cache, IConfiguration configuration)
		{
			_next = next;
			_cache = cache;
			_maxRequests = configuration.GetSection("RateLimitingOptions").GetValue<int>("MaxRequests");
			_timeWindow = configuration.GetSection("RateLimitingOptions").GetValue<int>("TimeWindow");
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var endpoint = context.GetEndpoint();

			if (_maxRequests <= 0 || _timeWindow <= 0)
			{
				await _next(context);
				return;
			}

			var key = GenerateClientKey(context);
			var clientStatistics = await GetClientStatisticsByKey(key);

			if (clientStatistics != null
				&& DateTime.UtcNow < clientStatistics.LastSuccessfulResponseTime.AddSeconds((double)_timeWindow)
				&& clientStatistics.NumberOfRequestsCompletedSuccessfully == _maxRequests)
			{
				throw new TooManyRequestsException();
			}

			await UpdateClientStatisticsStorage(key, _maxRequests);
			await _next(context);
		}

		private static string GenerateClientKey(HttpContext context) => $"{context.Request.Path}_{context.Connection.RemoteIpAddress}";

		private async Task<ClientStatistics> GetClientStatisticsByKey(string key) => await _cache.GetCacheValueAsync<ClientStatistics>(key);

		private async Task UpdateClientStatisticsStorage(string key, int maxRequests)
		{
			var clientStat = await _cache.GetCacheValueAsync<ClientStatistics>(key);

			if (clientStat != null)
			{
				clientStat.LastSuccessfulResponseTime = DateTime.UtcNow;

				if (clientStat.NumberOfRequestsCompletedSuccessfully == maxRequests)
					clientStat.NumberOfRequestsCompletedSuccessfully = 1;

				else
					clientStat.NumberOfRequestsCompletedSuccessfully++;

				await _cache.SetCahceValueAsync(key, clientStat);
			}
			else
			{
				var clientStatistics = new ClientStatistics
				{
					LastSuccessfulResponseTime = DateTime.UtcNow,
					NumberOfRequestsCompletedSuccessfully = 1
				};

				await _cache.SetCahceValueAsync(key, clientStatistics);
			}

		}
	}
}
