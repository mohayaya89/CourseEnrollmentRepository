using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent
{
    public record EnrollStudentCommand(Guid CourseId) : IRequest<Unit>;

}
