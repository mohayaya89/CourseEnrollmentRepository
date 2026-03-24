using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;

namespace CourseEnrollment.Domain.DomainEvents
{
    public class CourseOpenedEvent : IDomainEvent
    {
        public CourseOpenedEvent(Guid courseId)
        {
            CourseId = courseId;
            OccurredOn = DateTime.UtcNow;
        }

        public Guid CourseId { get; }

        public DateTime OccurredOn { get; }
    }
}
