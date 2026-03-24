using CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent;
using CourseEnrollment.Application.Features.Courses.Queries.GetCourseById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseEnrollment.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id));
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(Guid id)
        {
            await _mediator.Send(new EnrollStudentCommand(id));
            return RedirectToAction("Details", new { id });
        }
    }
}
