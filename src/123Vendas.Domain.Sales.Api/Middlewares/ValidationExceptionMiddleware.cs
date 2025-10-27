using _123Vendas.Domain.Sales.Api.Common;
using FluentValidation;
using System.Text.Json;

namespace _123Vendas.Domain.Sales.Api.Middlewares
{
    public class ValidationExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));

                var response = new ApiResponse
                {
                    Success = false,
                    Message = errorMessages
                };

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }
    }
}
