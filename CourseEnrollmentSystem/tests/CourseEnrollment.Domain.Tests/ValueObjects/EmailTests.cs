using CourseEnrollment.Domain.Exceptions;
using CourseEnrollment.Domain.ValueObjects;

namespace CourseEnrollment.Domain.Tests.ValueObjects
{
    [TestFixture]
    public class EmailTests
    {
        [Test]
        public void Create_ValidEmail_ReturnsEmailObject()
        {
            var email = Email.Create("User@Example.COM");
            Assert.That(email.Value, Is.EqualTo("user@example.com"));
        }

        [Test]
        public void Create_EmptyEmail_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() => Email.Create(""));
        }

        [Test]
        public void Create_InvalidEmail_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() => Email.Create("not-an-email"));
        }

        [Test]
        public void Equality_SameValue_AreEqual()
        {
            var a = Email.Create("test@example.com");
            var b = Email.Create("TEST@EXAMPLE.COM");
            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        public void Equality_DifferentValues_AreNotEqual()
        {
            var a = Email.Create("a@example.com");
            var b = Email.Create("b@example.com");
            Assert.That(a, Is.Not.EqualTo(b));
        }
    }
}
