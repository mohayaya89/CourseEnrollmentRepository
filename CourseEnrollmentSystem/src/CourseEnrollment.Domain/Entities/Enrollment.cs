using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;
using CourseEnrollment.Domain.DomainEvents;
using CourseEnrollment.Domain.Enums;
using CourseEnrollment.Domain.Exceptions;

namespace CourseEnrollment.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public Guid StudentId { get; private set; }
        public Guid CourseId { get; private set; }
        public DateTime EnrollmentDate { get; private set; }
        public EnrollmentStatus Status { get; private set; }


        private Enrollment() { } // For EF

        internal Enrollment(Guid studentId, Guid courseId)
        {
            if (studentId == Guid.Empty)
                throw new DomainException("StudentId cannot be empty.");

            if (courseId == Guid.Empty)
                throw new DomainException("CourseId cannot be empty.");

            StudentId = studentId;
            CourseId = courseId;
            EnrollmentDate = DateTime.UtcNow;
            Status = EnrollmentStatus.Active;
        }



        public void Cancel()
        {
            if (Status == EnrollmentStatus.Cancelled)
                throw new DomainException("Enrollment is already cancelled.");

            if (Status == EnrollmentStatus.Completed)
                throw new DomainException("Completed enrollment cannot be cancelled.");

            Status = EnrollmentStatus.Cancelled;

        }

        public void Complete() 
        { 
            if (Status == EnrollmentStatus.Active)
                throw new DomainException("Only active enrollment can be completed.");

            Status = EnrollmentStatus.Completed;
        }
    }

}
