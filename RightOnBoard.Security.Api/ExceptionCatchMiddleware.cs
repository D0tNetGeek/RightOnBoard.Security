using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RightOnBoard.Security.Api
{
    public class ExceptionCatchMiddleware
    {
        private readonly RequestDelegate _delegate;
        private readonly ILogger _logger;

        public ExceptionCatchMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionCatchMiddleware> logger)
        {
            _delegate = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _delegate(context);
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (Exception ex in e.LoaderExceptions)
                {
                    _logger.LogCritical(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
        }
    }
}
