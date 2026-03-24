using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;
using MediatR;

namespace CourseEnrollment.Infrastructure.DomainEvents
{
    public class DomainEventWrapper<T> : INotification
    where T : IDomainEvent
    {
        public T DomainEvent { get; }

        public DomainEventWrapper(T domainEvent)
        {
            DomainEvent = domainEvent;
        }
    }

}
