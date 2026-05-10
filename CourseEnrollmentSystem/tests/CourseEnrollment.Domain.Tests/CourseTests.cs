using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Enums;
using CourseEnrollment.Domain.Exceptions;

namespace CourseEnrollment.Domain.Tests
{
    [TestFixture]
    public class CourseTests
    {
        private static Course OpenCourse(int capacity = 10, int daysUntilDeadline = 30)
        {
            var course = new Course("Test Course", capacity, DateTime.UtcNow.AddDays(daysUntilDeadline));
            course.Open();
            return course;
        }

        // --- Constructor ---

        [Test]
        public void Constructor_ValidArguments_CreatesDraftCourse()
        {
            var course = new Course("Intro to C#", 20, DateTime.UtcNow.AddDays(10));

            Assert.That(course.Title, Is.EqualTo("Intro to C#"));
            Assert.That(course.Capacity, Is.EqualTo(20));
            Assert.That(course.Status, Is.EqualTo(CourseStatus.Draft));
        }

        [Test]
        public void Constructor_EmptyTitle_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                new Course("  ", 10, DateTime.UtcNow.AddDays(5)));
        }

        [Test]
        public void Constructor_ZeroCapacity_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                new Course("Title", 0, DateTime.UtcNow.AddDays(5)));
        }

        [Test]
        public void Constructor_PastDeadline_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() =>
                new Course("Title", 10, DateTime.UtcNow.AddDays(-1)));
        }

        // --- Open ---

        [Test]
        public void Open_DraftCourse_SetsStatusToOpen()
        {
            var course = new Course("Title", 5, DateTime.UtcNow.AddDays(10));
            course.Open();
            Assert.That(course.Status, Is.EqualTo(CourseStatus.Open));
        }

        [Test]
        public void Open_AlreadyOpenCourse_ThrowsDomainException()
        {
            var course = OpenCourse();
            Assert.Throws<DomainException>(() => course.Open());
        }

        // --- Close ---

        [Test]
        public void Close_OpenCourse_SetsStatusToClosed()
        {
            var course = OpenCourse();
            course.Close();
            Assert.That(course.Status, Is.EqualTo(CourseStatus.Closed));
        }

        [Test]
        public void Close_DraftCourse_ThrowsDomainException()
        {
            var course = new Course("Title", 5, DateTime.UtcNow.AddDays(10));
            Assert.Throws<DomainException>(() => course.Close());
        }

        // --- Reopen ---

        [Test]
        public void Reopen_ClosedCourse_SetsStatusToOpen()
        {
            var course = OpenCourse();
            course.Close();
            course.Reopen();
            Assert.That(course.Status, Is.EqualTo(CourseStatus.Open));
        }

        [Test]
        public void Reopen_OpenCourse_ThrowsDomainException()
        {
            var course = OpenCourse();
            Assert.Throws<DomainException>(() => course.Reopen());
        }

        // --- Archive ---

        [Test]
        public void Archive_OpenCourse_SetsStatusToArchived()
        {
            var course = OpenCourse();
            course.Archive();
            Assert.That(course.Status, Is.EqualTo(CourseStatus.Archived));
        }

        [Test]
        public void Archive_AlreadyArchivedCourse_ThrowsDomainException()
        {
            var course = OpenCourse();
            course.Archive();
            Assert.Throws<DomainException>(() => course.Archive());
        }

        // --- EnrollStudent ---

        [Test]
        public void EnrollStudent_OpenCourseWithCapacity_AddsEnrollment()
        {
            var course = OpenCourse(capacity: 5);
            course.EnrollStudent(Guid.NewGuid());
            Assert.That(course.Enrollments, Has.Count.EqualTo(1));
        }

        [Test]
        public void EnrollStudent_FullCourse_ThrowsDomainException()
        {
            var course = OpenCourse(capacity: 1);
            course.EnrollStudent(Guid.NewGuid());
            Assert.Throws<DomainException>(() => course.EnrollStudent(Guid.NewGuid()));
        }

        [Test]
        public void EnrollStudent_ClosedCourse_ThrowsDomainException()
        {
            var course = OpenCourse();
            course.Close();
            Assert.Throws<DomainException>(() => course.EnrollStudent(Guid.NewGuid()));
        }

        [Test]
        public void EnrollStudent_RaisesStudentEnrolledEvent()
        {
            var course = OpenCourse();
            course.EnrollStudent(Guid.NewGuid());
            Assert.That(course.DomainEvents, Has.Count.EqualTo(2)); // OpenedEvent + EnrolledEvent
        }
    }
}
