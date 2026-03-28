using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent
{
    public class EnrollStudentCommandValidator : AbstractValidator<EnrollStudentCommand>
    {
        public EnrollStudentCommandValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty();
        }
    }

}
