using CourseEnrollment.Domain.Enums;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourses
{
    public class CourseListDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public CourseStatus Status { get; init; }
    }
}
