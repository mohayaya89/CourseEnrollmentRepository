using CourseEnrollment.Domain.Common;

namespace CourseEnrollment.Domain.DomainEvents
{
    public class CourseReopenedEvent : IDomainEvent
    {
        public CourseReopenedEvent(Guid courseId)
        {
            CourseId = courseId;
            OccurredOn = DateTime.UtcNow;
        }

        public DateTime OccurredOn { get; }
        public Guid CourseId { get; }
    }
}