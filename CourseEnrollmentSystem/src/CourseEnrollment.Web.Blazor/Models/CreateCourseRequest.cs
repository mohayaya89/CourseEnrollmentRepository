namespace CourseEnrollment.Web.Blazor.Models;

public class CreateCourseRequest
{
    public string Title { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public DateTime EnrollmentDeadline { get; set; } = DateTime.UtcNow.AddDays(30);
}
