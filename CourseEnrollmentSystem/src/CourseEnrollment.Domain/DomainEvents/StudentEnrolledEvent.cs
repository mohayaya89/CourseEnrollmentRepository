using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;

namespace CourseEnrollment.Domain.DomainEvents
{
    public class StudentEnrolledEvent : IDomainEvent
    {
        public Guid StudentId { get; }
        public Guid EnrollmentId { get; }
        public DateTime OccurredOn { get; }

        public StudentEnrolledEvent(
            Guid studentId, 
            Guid enrollmentId)
        {
            StudentId = studentId;
            OccurredOn = DateTime.UtcNow;
            EnrollmentId = enrollmentId;
        }

    }
}
