using FluentValidation;

namespace CourseEnrollment.Application.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than zero.");

            RuleFor(x => x.EnrollmentDeadline)
                .GreaterThan(_ => DateTime.UtcNow).WithMessage("Enrollment deadline must be in the future.");
        }
    }
}
