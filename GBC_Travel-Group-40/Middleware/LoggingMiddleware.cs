namespace GBC_Travel_Group_40.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("Request: {method} {url} => started", context.Request.Method, context.Request.Path);

            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation("Request: {method} {url} => completed with {status}", context.Request.Method, context.Request.Path, context.Response.StatusCode);
            }
        }
    }

}
