﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CinemaApi.Middleware
{
    public class RequestTimeMiddleware: IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private Stopwatch _stopWatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopWatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            long elapsedMili = _stopWatch.ElapsedMilliseconds;
            if(elapsedMili/1000 > 4)
            {
                string message = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMili} ms";
                _logger.LogInformation(message);
            }
        }
    }
}
