using CourseEnrollment.Application.Features.Courses.Commands.CreateCourse;
using CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent;
using CourseEnrollment.Application.Features.Courses.Queries.GetCourseById;
using CourseEnrollment.Application.Features.Courses.Queries.GetCourses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseEnrollment.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var courses = await _mediator.Send(new GetCoursesQuery(), ct);
            return Ok(courses);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id), ct);
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseCommand request, CancellationToken ct)
        {
            var id = await _mediator.Send(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [Authorize(Policy = "StudentOnly")]
        [HttpPost("{id:guid}/enroll")]
        public async Task<IActionResult> Enroll(Guid id, CancellationToken ct)
        {
            await _mediator.Send(new EnrollStudentCommand(id), ct);
            return NoContent();
        }

        //public record CreateCourseRequest(string Title, int Capacity, DateTime EnrollmentDeadline);
    }
}
