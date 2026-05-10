namespace CourseEnrollment.Web.Blazor.Models;

public class CourseDetailModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Capacity { get; init; }
    public string Status { get; init; } = string.Empty;
    public int ActiveEnrollments { get; init; }
}
