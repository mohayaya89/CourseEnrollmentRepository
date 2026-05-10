namespace CourseEnrollment.Web.Blazor.Models;

public class CourseListModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
