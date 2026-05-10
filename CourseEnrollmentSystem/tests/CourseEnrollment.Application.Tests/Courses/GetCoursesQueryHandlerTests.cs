using AutoMapper;
using CourseEnrollment.Application.Features.Courses.Queries.GetCourses;
using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Interfaces;
using NSubstitute;

namespace CourseEnrollment.Application.Tests.Courses
{
    [TestFixture]
    public class GetCoursesQueryHandlerTests
    {
        private ICourseRepository _repository = null!;
        private IMapper _mapper = null!;
        private GetCoursesQueryHandler _handler = null!;

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<ICourseRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetCoursesQueryHandler(_repository, _mapper);
        }

        [Test]
        public async Task Handle_CallsGetAllAsync()
        {
            _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Course>());
            _mapper.Map<IEnumerable<CourseListDto>>(Arg.Any<object>()).Returns(Enumerable.Empty<CourseListDto>());

            await _handler.Handle(new GetCoursesQuery(), CancellationToken.None);

            await _repository.Received(1).GetAllAsync(Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_ReturnsEmptyList_WhenNoCoursesExist()
        {
            _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Course>());
            _mapper.Map<IEnumerable<CourseListDto>>(Arg.Any<object>()).Returns(Enumerable.Empty<CourseListDto>());

            var result = await _handler.Handle(new GetCoursesQuery(), CancellationToken.None);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task Handle_PassesCoursesToMapper()
        {
            var courses = new List<Course>
            {
                new Course("C# Basics", 10, DateTime.UtcNow.AddDays(30))
            };
            _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(courses);
            _mapper.Map<IEnumerable<CourseListDto>>(courses)
                .Returns(new[] { new CourseListDto { Title = "C# Basics" } });

            var result = (await _handler.Handle(new GetCoursesQuery(), CancellationToken.None)).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("C# Basics"));
        }
    }
}
