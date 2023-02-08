using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Visma.Bootcamp.eShop.ApplicationCore.Exceptions;

namespace Visma.Bootcamp.eShop.ApplicationCore.Infrastructure.Middlewates
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context) 
        {
            try 
            {
                await _next.Invoke(context);
            }
            catch(NotFoundExceptions nfe)
            {
                _logger.LogError(nfe, nfe.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(nfe.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}