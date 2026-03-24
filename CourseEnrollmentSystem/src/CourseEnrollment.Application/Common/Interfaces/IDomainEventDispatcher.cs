using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;

namespace CourseEnrollment.Application.Common.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents);
    }

}
