using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Interfaces;
using CourseEnrollment.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CourseEnrollment.Infrastructure.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CourseEnrollmentContext _context;

        public StudentRepository(CourseEnrollmentContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => await _context.Students.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public async Task<List<Student>> GetAllAsync(CancellationToken cancellationToken)
            => await _context.Students.ToListAsync(cancellationToken);

        public async Task AddAsync(Student student, CancellationToken cancellationToken)
            => await _context.Students.AddAsync(student, cancellationToken);
    }
}
