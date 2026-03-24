using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Application.Common.Exceptions;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent
{
    public class EnrollStudentCommandHandler
    : IRequestHandler<EnrollStudentCommand, Unit>
    {
        private readonly ICourseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public EnrollStudentCommandHandler(
            ICourseRepository repository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(
            EnrollStudentCommand request,
            CancellationToken ct)
        {
            var course = await _repository.GetByIdAsync(request.CourseId, ct);

            if (course is null)
                throw new NotFoundException("Course not found.");

            course.EnrollStudent(_currentUser.UserId);

            await _unitOfWork.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }

}
