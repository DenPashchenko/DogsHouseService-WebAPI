using DogsHouseService.Application.Common.Exceptions;
using DogsHouseService.WebApi.Middleware.Extensions;
using DogsHouseService.WebApi.Middleware.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace DogsHouseService.Tests.MiddlewareTests
{
	public class RateLimitingMiddlewareTests
	{
		private DefaultHttpContext _defaultHttpContext;
		private IDistributedCache _cache;
		private IConfiguration _configuration;

		public RateLimitingMiddlewareTests()
		{
			_defaultHttpContext = new DefaultHttpContext();
			var opts = Options.Create(new MemoryDistributedCacheOptions());
			_cache = new MemoryDistributedCache(opts);

			var configurationBuilder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddEnvironmentVariables();

			_configuration = configurationBuilder.Build();
			//There are such parameters in appsettings: "MaxRequests": 10, "TimeWindow": 1 sec
		}

		[Fact]
		public async Task WhenRateLimitingMiddlewareInvoked_NextDelegateIsCalled()
		{
			const string expectedOutput = "Request handed over to next request delegate";
			_defaultHttpContext.Response.Body = new MemoryStream();
			_defaultHttpContext.Request.Path = "/dogs";
			_defaultHttpContext.Connection.RemoteIpAddress = new System.Net.IPAddress(16885952);
			var endpoint = CreateEndpoint();
			_defaultHttpContext.SetEndpoint(endpoint);
			var middlewareInstance = new RateLimitingMiddleware(next: (innerHttpContext) =>
			{
				innerHttpContext.Response.WriteAsync(expectedOutput);
				return Task.CompletedTask;
			}, _cache, _configuration);

			await middlewareInstance.InvokeAsync(_defaultHttpContext);

			_defaultHttpContext.Response.Body.Seek(0, SeekOrigin.Begin);
			var body = new StreamReader(_defaultHttpContext.Response.Body).ReadToEnd();
			Assert.Equal(expectedOutput, body);
		}

		[Fact]
		public async Task WhenDogsEndpointInvokedOnlyOnce_StatusCodeOkIsReturned()
		{
			_defaultHttpContext.Response.Body = new MemoryStream();
			_defaultHttpContext.Request.Path = "/dogs";
			_defaultHttpContext.Connection.RemoteIpAddress = new System.Net.IPAddress(16885952);
			var endpoint = CreateEndpoint();
			_defaultHttpContext.SetEndpoint(endpoint);
			var middlewareInstance = new RateLimitingMiddleware(next: (innerHttpContext) =>
			{
				return Task.CompletedTask;
			}, _cache, _configuration);

			await middlewareInstance.InvokeAsync(_defaultHttpContext);

			Assert.Equal(200, _defaultHttpContext.Response.StatusCode);
		}

		[Fact]
		public async Task WhenDogsEndpointInvokedForElevenTimeInOneSecond_TooManyRequestsExceptionThrown()
		{
			_defaultHttpContext.Response.Body = new MemoryStream();
			_defaultHttpContext.Request.Path = "/dogs";
			_defaultHttpContext.Connection.RemoteIpAddress = new System.Net.IPAddress(16885952);
			var endpoint = CreateEndpoint();
			_defaultHttpContext.SetEndpoint(endpoint);
			var clientStatistics = new ClientStatistics
			{
				LastSuccessfulResponseTime = DateTime.UtcNow.AddSeconds(-0.5),
				NumberOfRequestsCompletedSuccessfully = 10
			};
			await _cache.SetCahceValueAsync("/dogs_192.168.1.1", clientStatistics);
			var middlewareInstance = new RateLimitingMiddleware(next: (innerHttpContext) =>
			{
				return Task.CompletedTask;
			}, _cache, _configuration);

			await Assert.ThrowsAsync<TooManyRequestsException>(async () =>
				await middlewareInstance.InvokeAsync(_defaultHttpContext));
		}

		[Fact]
		public async Task WhenDogsEndpointInvokedForElevenTimeAfterOneSecond_StatusCodeOkIsReturned()
		{
			_defaultHttpContext.Response.Body = new MemoryStream();
			_defaultHttpContext.Request.Path = "/dogs";
			_defaultHttpContext.Connection.RemoteIpAddress = new System.Net.IPAddress(16885952);
			var endpoint = CreateEndpoint();
			_defaultHttpContext.SetEndpoint(endpoint);
			var clientStatistics = new ClientStatistics
			{
				LastSuccessfulResponseTime = DateTime.UtcNow.AddSeconds(-1.1),
				NumberOfRequestsCompletedSuccessfully = 10
			};
			await _cache.SetCahceValueAsync("/dogs_192.168.1.1", clientStatistics);
			var middlewareInstance = new RateLimitingMiddleware(next: (innerHttpContext) =>
			{
				return Task.CompletedTask;
			}, _cache, _configuration);
			await middlewareInstance.InvokeAsync(_defaultHttpContext);
			Assert.Equal(200, _defaultHttpContext.Response.StatusCode);
		}
		private Endpoint CreateEndpoint(params object[] metadata) => new Endpoint(context => Task.CompletedTask, new EndpointMetadataCollection(metadata), string.Empty);

	}
}
