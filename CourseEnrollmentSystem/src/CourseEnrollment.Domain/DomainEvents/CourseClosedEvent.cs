using CourseEnrollment.Domain.Common;

namespace CourseEnrollment.Domain.DomainEvents
{
    internal class CourseClosedEvent : IDomainEvent
    {
        public CourseClosedEvent(Guid couseId)
        {
            CouseId = couseId;
            OccurredOn = DateTime.UtcNow;
        }
        public DateTime OccurredOn { get; }
        public Guid CouseId { get; }
    }
}