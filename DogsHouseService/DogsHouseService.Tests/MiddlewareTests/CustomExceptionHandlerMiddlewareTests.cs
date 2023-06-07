using DogsHouseService.Application.Common.Exceptions;
using DogsHouseService.WebApi.Middleware.CustomExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DogsHouseService.Application.Properties;
using System.Net;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace DogsHouseService.Tests.MiddlewareTests
{
	public class CustomExceptionHandlerMiddlewareTests
	{
		private DefaultHttpContext _defaultHttpContext;
		private MemoryStream _responseStream;

		public CustomExceptionHandlerMiddlewareTests()
		{
			_defaultHttpContext = new DefaultHttpContext();
			_responseStream = new MemoryStream();
			_defaultHttpContext.Response.Body = _responseStream;
		}

		[Fact]
		public async Task Invoke_NotFoundException_HandlesExceptionAndSetsResponse()
		{
			var nextCalled = false;
			var entityName = "Name";
			var entityId = "Id";
			var expectedDetail = string.Format(Resources.EntityNotFound, entityName, entityId);

			var middleware = new CustomExceptionHandlerMiddleware((innerContext) =>
			{
				nextCalled = true;
				throw new NotFoundException(entityName, entityId);
			});

			await middleware.Invoke(_defaultHttpContext);
			_responseStream.Seek(0, SeekOrigin.Begin);
			var responseContent = await new StreamReader(_responseStream).ReadToEndAsync();
			var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

			Assert.True(nextCalled);
			Assert.Equal((int)HttpStatusCode.NotFound, _defaultHttpContext.Response.StatusCode);
			Assert.NotNull(problemDetails);
			Assert.Equal(WebApi.Properties.Resources.NotFound, problemDetails?.Title);
			Assert.Equal(expectedDetail, problemDetails?.Detail);
			Assert.Equal((int)HttpStatusCode.NotFound, problemDetails?.Status);
		}

		[Fact]
		public async Task Invoke_ValidationException_HandlesExceptionAndSetsResponse()
		{
			var nextCalled = false;
			var middleware = new CustomExceptionHandlerMiddleware((innerContext) =>
			{
				nextCalled = true;
				throw new ValidationException("ValidationException message");
			});

			await middleware.Invoke(_defaultHttpContext);
			_responseStream.Seek(0, SeekOrigin.Begin);
			var responseContent = await new StreamReader(_responseStream).ReadToEndAsync();
			var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

			Assert.True(nextCalled);
			Assert.Equal((int)HttpStatusCode.BadRequest, _defaultHttpContext.Response.StatusCode);
			Assert.NotNull(problemDetails);
			Assert.Equal(WebApi.Properties.Resources.ValidationErrors, problemDetails?.Title);
			Assert.Equal("ValidationException message", problemDetails?.Detail);
			Assert.Equal((int)HttpStatusCode.BadRequest, problemDetails?.Status);
		}

		[Fact]
		public async Task Invoke_TooManyRequestsException_HandlesExceptionAndSetsResponse()
		{
			var nextCalled = false;
			var middleware = new CustomExceptionHandlerMiddleware((innerContext) =>
			{
				nextCalled = true;
				throw new TooManyRequestsException();
			});

			await middleware.Invoke(_defaultHttpContext);
			_responseStream.Seek(0, SeekOrigin.Begin);
			var responseContent = await new StreamReader(_responseStream).ReadToEndAsync();
			var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

			Assert.True(nextCalled);
			Assert.Equal((int)HttpStatusCode.TooManyRequests, _defaultHttpContext.Response.StatusCode);
			Assert.NotNull(problemDetails);
			Assert.Equal(WebApi.Properties.Resources.TooManyRequests, problemDetails?.Title);
			Assert.Equal(Resources.TooManyRequests, problemDetails?.Detail);
			Assert.Equal((int)HttpStatusCode.TooManyRequests, problemDetails?.Status);
		}

		[Fact]
		public async Task Invoke_OtherException_HandlesExceptionAndSetsResponse()
		{
			var nextCalled = false;
			var middleware = new CustomExceptionHandlerMiddleware((innerContext) =>
			{
				nextCalled = true;
				throw new Exception("Exception message");
			});

			await middleware.Invoke(_defaultHttpContext);
			_responseStream.Seek(0, SeekOrigin.Begin);
			var responseContent = await new StreamReader(_responseStream).ReadToEndAsync();
			var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(responseContent);

			Assert.True(nextCalled);
			Assert.NotEqual(0, _defaultHttpContext.Response.StatusCode);
			Assert.NotNull(problemDetails);
			Assert.Equal("Exception message", problemDetails?.Detail);
		}
	}
}
