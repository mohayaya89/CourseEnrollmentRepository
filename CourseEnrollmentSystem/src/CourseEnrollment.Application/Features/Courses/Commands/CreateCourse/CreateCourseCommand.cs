using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Commands.CreateCourse
{
    public record CreateCourseCommand(
        string Title,
        int Capacity,
        DateTime EnrollmentDeadline) : IRequest<Guid>;
}
