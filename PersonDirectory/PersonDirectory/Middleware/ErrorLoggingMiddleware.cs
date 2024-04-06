using System.Text;

namespace PersonDirectory.API.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogRawErrorDetails(context, ex);

                throw;
            }
        }

        private void LogRawErrorDetails(HttpContext context, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Error occurred at: {DateTime.Now}");
            sb.AppendLine($"Request Method: {context.Request.Method}");
            sb.AppendLine($"Request Path: {context.Request.Path}");

            if (context.Request.Body != null)
            {
                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    sb.AppendLine($"Request Body: {reader.ReadToEndAsync()}");
                }
            }

            sb.AppendLine($"Exception: {ex.ToString()}");

            _logger.LogError(sb.ToString());
        }
    }
}
