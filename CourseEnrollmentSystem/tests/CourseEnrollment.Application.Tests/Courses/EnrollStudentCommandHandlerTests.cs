using CourseEnrollment.Application.Common.Exceptions;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Application.Features.Courses.Commands.EnrollStudent;
using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Interfaces;
using NSubstitute;

namespace CourseEnrollment.Application.Tests.Courses
{
    [TestFixture]
    public class EnrollStudentCommandHandlerTests
    {
        private ICourseRepository _courseRepo = null!;
        private IStudentRepository _studentRepo = null!;
        private IUnitOfWork _unitOfWork = null!;
        private ICurrentUserService _currentUser = null!;
        private EnrollStudentCommandHandler _handler = null!;

        private static readonly Guid StudentId = Guid.NewGuid();
        private static readonly Guid CourseId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _courseRepo = Substitute.For<ICourseRepository>();
            _studentRepo = Substitute.For<IStudentRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _currentUser = Substitute.For<ICurrentUserService>();
            _currentUser.UserId.Returns(StudentId);

            _handler = new EnrollStudentCommandHandler(
                _courseRepo, _studentRepo, _unitOfWork, _currentUser);
        }

        [Test]
        public async Task Handle_ValidStudentAndCourse_EnrollsAndSaves()
        {
            var student = new Student("John", "Doe", Domain.ValueObjects.Email.Create("john@example.com"));
            var course = new Course("C# 101", 5, DateTime.UtcNow.AddDays(30));
            course.Open();

            _studentRepo.GetByIdAsync(StudentId, Arg.Any<CancellationToken>()).Returns(student);
            _courseRepo.GetByIdAsync(CourseId, Arg.Any<CancellationToken>()).Returns(course);

            await _handler.Handle(new EnrollStudentCommand(CourseId), CancellationToken.None);

            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            Assert.That(course.Enrollments, Has.Count.EqualTo(1));
        }

        [Test]
        public void Handle_StudentNotFound_ThrowsNotFoundException()
        {
            _studentRepo.GetByIdAsync(StudentId, Arg.Any<CancellationToken>())
                .Returns((Student?)null);

            Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new EnrollStudentCommand(CourseId), CancellationToken.None));
        }

        [Test]
        public void Handle_CourseNotFound_ThrowsNotFoundException()
        {
            var student = new Student("Jane", "Smith", Domain.ValueObjects.Email.Create("jane@example.com"));
            _studentRepo.GetByIdAsync(StudentId, Arg.Any<CancellationToken>()).Returns(student);
            _courseRepo.GetByIdAsync(CourseId, Arg.Any<CancellationToken>())
                .Returns((Course?)null);

            Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new EnrollStudentCommand(CourseId), CancellationToken.None));
        }
    }
}
