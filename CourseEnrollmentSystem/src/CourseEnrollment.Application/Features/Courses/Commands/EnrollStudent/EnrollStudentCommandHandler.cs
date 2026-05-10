using CourseEnrollment.Application.Common.Exceptions;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent
{
    public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, Unit>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public EnrollStudentCommandHandler(
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(EnrollStudentCommand request, CancellationToken ct)
        {
            var student = await _studentRepository.GetByIdAsync(_currentUser.UserId, ct);
            if (student is null)
                throw new NotFoundException("Student not found.");

            var course = await _courseRepository.GetByIdAsync(request.CourseId, ct);
            if (course is null)
                throw new NotFoundException("Course not found.");

            course.EnrollStudent(student.Id);

            await _unitOfWork.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
