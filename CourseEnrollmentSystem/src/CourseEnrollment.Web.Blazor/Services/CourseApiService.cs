using System.Net.Http.Json;
using CourseEnrollment.Web.Blazor.Models;
using Microsoft.Extensions.Logging;

namespace CourseEnrollment.Web.Blazor.Services;

public class CourseApiService
{
    private readonly HttpClient _http;
    private readonly ILogger<CourseApiService> _logger;

    public CourseApiService(HttpClient http, ILogger<CourseApiService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<CourseListModel[]?> GetCoursesAsync()
    {
        try { return await _http.GetFromJsonAsync<CourseListModel[]>("api/courses"); }
        catch (Exception ex) { _logger.LogError(ex, "GET api/courses failed"); return null; }
    }

    public async Task<CourseDetailModel?> GetCourseByIdAsync(Guid id)
    {
        try { return await _http.GetFromJsonAsync<CourseDetailModel>($"api/courses/{id}"); }
        catch (Exception ex) { _logger.LogError(ex, "GET api/courses/{Id} failed", id); return null; }
    }

    public async Task<Guid?> CreateCourseAsync(CreateCourseRequest request)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/courses", request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("POST api/courses returned {StatusCode}", response.StatusCode);
                return null;
            }

            var result = await response.Content.ReadFromJsonAsync<CreateCourseIdResult>();
            return result?.Id;
        }
        catch (Exception ex) { _logger.LogError(ex, "POST api/courses failed"); return null; }
    }

    public async Task<bool> EnrollAsync(Guid courseId)
    {
        try
        {
            var response = await _http.PostAsync($"api/courses/{courseId}/enroll", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex) { _logger.LogError(ex, "POST api/courses/{Id}/enroll failed", courseId); return false; }
    }

    private record CreateCourseIdResult(Guid Id);
}
