using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Common;
using MediatR;

namespace CourseEnrollment.Infrastructure.DomainEvents
{
    public class MediatRDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public MediatRDomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(new DomainEventWrapper<IDomainEvent>(domainEvent));
            }
        }
    }

}
