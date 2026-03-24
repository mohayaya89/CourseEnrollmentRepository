using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourseById
{
    public record GetCourseByIdQuery(Guid Id) : IRequest<CourseDto>;

}
