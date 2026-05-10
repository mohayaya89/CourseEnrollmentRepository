using CourseEnrollment.Application.Features.Courses.Commands.CreateCourse;
using CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent;
using CourseEnrollment.Application.Features.Courses.Queries.GetCourseById;
using CourseEnrollment.Application.Features.Courses.Queries.GetCourses;
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

        public async Task<IActionResult> Index()
        {
            var courses = await _mediator.Send(new GetCoursesQuery());
            return View(courses);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id));
            return View(course);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseCommand command)
        {
            var id = await _mediator.Send(command);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(Guid id)
        {
            try
            {
                await _mediator.Send(new EnrollStudentCommand(id));
                TempData["Success"] = "Successfully enrolled!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Details", new { id });
        }
    }
}
