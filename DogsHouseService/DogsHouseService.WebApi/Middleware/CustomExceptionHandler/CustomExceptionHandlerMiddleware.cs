using DogsHouseService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Net;
using System.Resources;
using System.Text.Json;
using DogsHouseService.WebApi.Properties;

namespace DogsHouseService.WebApi.Middleware.CustomExceptionHandler
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var problemTitle = string.Empty;
            var exceptionMessage = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    exceptionMessage = validationException.Message;
                    problemTitle = Resources.ValidationErrors;
                    break;

                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    exceptionMessage = notFoundException.Message;
                    problemTitle = Resources.NotFound;
                    break;

				case TooManyRequestsException tooManyRequestsException:
					code = HttpStatusCode.TooManyRequests;
					exceptionMessage = tooManyRequestsException.Message;
					problemTitle = Resources.TooManyRequests;
					break;
			}

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (exceptionMessage == string.Empty)
            {
                exceptionMessage = exception.Message;
            }
            ProblemDetails problemDetails = new()
            {
                Type = @"https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = (int)code,
                Title = problemTitle,
                Detail = exceptionMessage
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
