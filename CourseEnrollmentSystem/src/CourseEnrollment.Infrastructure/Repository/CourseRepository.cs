using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Interfaces;
using CourseEnrollment.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollment.Infrastructure.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseEnrollmentContext _context;

        public CourseRepository(CourseEnrollmentContext context)
        {
            _context = context;
        }

        public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Courses
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task AddAsync(Course course, CancellationToken cancellationToken)
        {
            await _context.Courses.AddAsync(course, cancellationToken);
        }

        public async Task<List<Course>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Courses.ToListAsync(cancellationToken);
        }
    }

}
