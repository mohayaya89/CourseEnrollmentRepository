using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourses
{
    public record GetCoursesQuery : IRequest<IEnumerable<CourseListDto>>
    {
    }
}
