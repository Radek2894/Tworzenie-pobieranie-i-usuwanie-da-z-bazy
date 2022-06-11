using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radzio.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware                      // klasa do obsługi i wyłapywania wyjątków 
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) // konstruktor
            {
                _logger = logger;
            }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
          try

            {
               await next.Invoke(context);
            }
            catch(NotFoundException notFoundException)
            {
               context.Response.StatusCode = 404;
               await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;    //nadpisanie kodu statusu odpowiedzi oraz odpowiedzi otrzymanej przez klienta
                await context.Response.WriteAsync("Coś poszło nie tak."); // dajemy await w przypadku metody asynchronicznej
            }
        } 
    }
}
