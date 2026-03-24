using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Entities;

namespace CourseEnrollment.Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task AddAsync(Course course, CancellationToken cancellationToken);
    }
}
