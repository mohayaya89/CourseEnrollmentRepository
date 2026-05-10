using CourseEnrollment.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Domain.Common
{
    public abstract class BasePerson
    {
        public Guid Id { get; protected set; }

        public FullName? Name { get; protected set; }
        public Email? Email { get; protected set; }
        public string? PhoneNumber { get; protected set; }
        public DateTime? DateOfBirth { get; protected set; }

        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected BasePerson()
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
