using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Common;
using CourseEnrollment.Domain.DomainEvents;
using CourseEnrollment.Domain.Enums;
using CourseEnrollment.Domain.Exceptions;

namespace CourseEnrollment.Domain.Entities
{
    public class Course : BaseEntity
    {
        private readonly List<Enrollment> _enrollments = new();

        public string Title { get; private set; } = string.Empty;
        public int Capacity { get; private set; }
        public DateTime EnrollmentDeadline { get; private set; }
        public CourseStatus Status { get; private set; }

        public IReadOnlyCollection<Enrollment> Enrollments => _enrollments.AsReadOnly();

        private Course() { } // For EF

        public Course(string title, int capacity, DateTime enrollmentDeadline)
        {
            if (capacity <= 0)
                throw new DomainException("Capacity must be greater than zero.");

            if (enrollmentDeadline <= DateTime.UtcNow)
                throw new DomainException("Enrollment deadline must be in the future.");

            Title = title;
            Capacity = capacity;
            EnrollmentDeadline = enrollmentDeadline;
            Status = CourseStatus.Draft;
        }

        public void EnrollStudent(Guid studentId)
        {
            if (Status != CourseStatus.Open)
                throw new DomainException("Course is not open for enrollment.");

            if (_enrollments.Count(s => s.Status == EnrollmentStatus.Active) >= Capacity)
                throw new DomainException("Course is full.");

            if (DateTime.UtcNow > EnrollmentDeadline)
                throw new DomainException("Enrollment deadline passed.");

            var enrollment = new Enrollment(studentId, Id);
            _enrollments.Add(enrollment);

            AddDomainEvent(new StudentEnrolledEvent(studentId, Id));
        }


        public void Open()
        {
            if (Status != CourseStatus.Draft)
                throw new DomainException("Only draft course can be opened.");

            Status = CourseStatus.Open;

            AddDomainEvent(new CourseOpenedEvent(Id));
        }

        public void Reopen()
        {
            if (Status != CourseStatus.Closed)
                throw new DomainException("Only closed courses can be reopened.");

            if (_enrollments.Count(e => e.Status == EnrollmentStatus.Active) >= Capacity)
                throw new DomainException("Course is still full.");

            if (DateTime.UtcNow > EnrollmentDeadline)
                throw new DomainException("Cannot reopen after enrollment deadline.");

            Status = CourseStatus.Open;

            AddDomainEvent(new CourseReopenedEvent(Id));
        }


        public void Close()
        {
            if (Status != CourseStatus.Open)
                throw new DomainException("Only open course can be closed.");

            Status = CourseStatus.Closed;

            AddDomainEvent(new CourseClosedEvent(Id));
        }

        public void Archive()
        {
            if (Status == CourseStatus.Archived)
                throw new DomainException("Course already archived.");

            Status = CourseStatus.Archived;
        }

    }


}
