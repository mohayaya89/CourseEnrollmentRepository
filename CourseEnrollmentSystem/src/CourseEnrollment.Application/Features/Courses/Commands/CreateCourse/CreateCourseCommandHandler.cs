using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Guid>
    {
        private readonly ICourseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCourseCommandHandler(ICourseRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCourseCommand request, CancellationToken ct)
        {
            var course = new Course(request.Title, request.Capacity, request.EnrollmentDeadline);
            await _repository.AddAsync(course, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return course.Id;
        }
    }
}
