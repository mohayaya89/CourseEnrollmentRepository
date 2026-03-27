using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;
using CourseEnrollment.Domain.Exceptions;
using CourseEnrollment.Domain.ValueObjects;

namespace CourseEnrollment.Domain.Entities
{
    public class Student : BaseEntity
    {
        public FullName? Name { get; private set; }
        public Email? Email { get; private set; }
        public bool IsActive { get; private set; }

        private Student() { } // For EF

        public Student(string firstName, string lastName, Email email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First name is required.");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Email is required.");

            Name = FullName.Create(firstName, lastName);
            Email = email;
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DomainException("Student is already inactive.");

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                throw new DomainException("Student is already active.");

            IsActive = true;
        }

        public void ChangeEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new DomainException("Email is required.");

            Email = Email.Create(newEmail);
        }
    }


}
