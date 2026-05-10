using CourseEnrollment.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseEnrollment.Infrastructure.DomainEvents
{
    public class DomainEventLoggingHandler : INotificationHandler<DomainEventWrapper<IDomainEvent>>
    {
        private readonly ILogger<DomainEventLoggingHandler> _logger;

        public DomainEventLoggingHandler(ILogger<DomainEventLoggingHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventWrapper<IDomainEvent> notification, CancellationToken cancellationToken)
        {
            var eventType = notification.DomainEvent.GetType().Name;
            _logger.LogInformation("Domain event raised: {EventType} at {OccurredOn}",
                eventType, notification.DomainEvent.OccurredOn);
            return Task.CompletedTask;
        }
    }
}
