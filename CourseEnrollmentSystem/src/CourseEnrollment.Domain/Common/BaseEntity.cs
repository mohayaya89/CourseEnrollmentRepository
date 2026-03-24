using System;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }

}
