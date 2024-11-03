using Questao5.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Questao5.Application.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var baseExceptionType = exception.GetType().GetGenericTypeDefinition();
            if (baseExceptionType == typeof(BaseException<>))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                
                var response = exception.GetType().GetProperty("Response")?.GetValue(exception);
                var result = JsonSerializer.Serialize(response);

                return context.Response.WriteAsync(result);
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = "An unexpected error occurred." }));
        }
    }
}
