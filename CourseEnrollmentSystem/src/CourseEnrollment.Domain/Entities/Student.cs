using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;
using CourseEnrollment.Domain.Enums;
using CourseEnrollment.Domain.Exceptions;
using CourseEnrollment.Domain.ValueObjects;

namespace CourseEnrollment.Domain.Entities
{
    /// <summary>Student domain entity.</summary>
    public class Student : BasePerson
    {
        public int StudentNumber { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public int ProgramId { get; set; }
        public StudentStatus Status { get; set; } = StudentStatus.Inactive;
        public bool IsActive { get; private set; }

        /// <summary>FK to the Identity ApplicationUser that owns this student profile. Null until linked.</summary>
        public Guid? UserId { get; private set; }

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
            Status = StudentStatus.Active;
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

        /// <summary>
        /// Associates this student with an Identity user. Can only be called once;
        /// throws <see cref="DomainException"/> if already linked.
        /// </summary>
        public void LinkToUser(Guid userId)
        {
            if (UserId.HasValue)
                throw new DomainException("Student is already linked to an identity user.");

            UserId = userId;
        }
    }


}
