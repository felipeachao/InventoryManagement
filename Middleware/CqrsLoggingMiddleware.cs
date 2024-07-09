using System.Text;
using InventoryManagement.Data;
using InventoryManagement.Data.Entities;

namespace InventoryManagement.Middleware
{
    public class CqrsLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CqrsLoggingMiddleware> _logger;

        public CqrsLoggingMiddleware(RequestDelegate next, ILogger<CqrsLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, InventoryDbContext dbContext)
        {
            var request = context.Request;

            if (request.Path.StartsWithSegments("/api"))
            {
                var requestBodyContent = await ReadRequestBodyAsync(request);
                var originalResponseBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    var responseBodyContent = await ReadResponseBodyAsync(context.Response);
                    await LogRequestResponseAsync(dbContext, request.Path, requestBodyContent, responseBodyContent);

                    await responseBody.CopyToAsync(originalResponseBodyStream);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            using (var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            }
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return body;
        }

        private async Task LogRequestResponseAsync(InventoryDbContext dbContext, string path, string requestBody, string responseBody)
        {
            var log = new ErrorLog
            {
                Message = $"Request: {requestBody}, Response: {responseBody}",
                StackTrace = path,
                Date = DateTime.UtcNow
            };

            dbContext.ErrorLogs.Add(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
