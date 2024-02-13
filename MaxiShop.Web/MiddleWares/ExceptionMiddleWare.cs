using MaxiShop.Application.Exceptions;
using MaxiShop.Web.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace MaxiShop.Web.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleWare(RequestDelegate next)
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
        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            CustomProblemDetails problem = new();

            switch (ex)
            {
                case BadRequestException BadRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    problem = new CustomProblemDetails()
                    {
                        Title = BadRequestException.Message,
                        status = (int)statusCode,
                        Type = nameof(BadRequestException),
                        Details = BadRequestException.InnerException?.Message,
                        Errors = BadRequestException.ValidationErrors

                    };
                    break;
            }
            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(problem);
        }
    }
}
