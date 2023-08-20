using OnlineShop.Domain.Interfaces;

namespace OnlineShop.WebApi.Middleware
{
    public class TransitionsCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TransitionsCounterMiddleware> _logger;
        private readonly ITransitionCounterService _transitionCounter;

        public TransitionsCounterMiddleware(
            RequestDelegate next,
            ILogger<TransitionsCounterMiddleware> logger,
            ITransitionCounterService transitionCounter)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _transitionCounter = transitionCounter;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue)
                _transitionCounter.AddPath(context.Request.Path.Value);
            _logger.LogInformation("Request Method: {Method}", context.Request.Path);
            await _next(context);
        }
    }
}
