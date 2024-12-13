namespace ApiLayer.Middlewares
{
    public class CustomMiddleware
    {
        private const string requiredHeader = "X-Custom-Header";
        private const string customHeader = "X-Response-Header";
        private const string customHeaderValue = "Response Value";

        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(requiredHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Missing required header: X-Custom-Header");
                return;
            }

            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append(customHeader, customHeaderValue);
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
