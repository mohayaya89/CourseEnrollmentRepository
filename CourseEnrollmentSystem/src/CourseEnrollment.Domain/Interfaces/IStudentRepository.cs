using CourseEnrollment.Domain.Entities;

namespace CourseEnrollment.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Student>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Student student, CancellationToken cancellationToken);
    }
}
