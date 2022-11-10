using Desafio.Contas.Application.Validation;
using System.Net;
using System.Text.Json;

namespace Desafio.Contas.Api.Middlewares
{
    /// <summary>
    /// Middleware utilizado para capturar falhas na execução de operações e formatá-las em um padrão único
    /// </summary>
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// </summary>
        /// <param name="next"></param>
        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                ErrorResponse responseModel;

                switch (error)
                {
                    case ValidationException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseModel = new ErrorResponse(error.Message, e.Errors.ToArray());
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        responseModel = new ErrorResponse(error.Message, new string[0]);
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);

                await response.WriteAsync(result);
            }
        }
    }

    public record ErrorResponse(string Message, string[] Errors)
    { 
    }
}
