using CourseEnrollment.Domain.Exceptions;
using CourseEnrollment.Domain.ValueObjects;

namespace CourseEnrollment.Domain.Tests.ValueObjects
{
    [TestFixture]
    public class FullNameTests
    {
        [Test]
        public void Create_ValidNames_ReturnsFullName()
        {
            var name = FullName.Create("John", "Doe");
            Assert.That(name.FirstName, Is.EqualTo("John"));
            Assert.That(name.LastName, Is.EqualTo("Doe"));
        }

        [Test]
        public void Create_EmptyFirstName_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() => FullName.Create("", "Doe"));
        }

        [Test]
        public void Create_EmptyLastName_ThrowsDomainException()
        {
            Assert.Throws<DomainException>(() => FullName.Create("John", "  "));
        }

        [Test]
        public void Create_TrimsWhitespace()
        {
            var name = FullName.Create("  Alice  ", "  Smith  ");
            Assert.That(name.FirstName, Is.EqualTo("Alice"));
            Assert.That(name.LastName, Is.EqualTo("Smith"));
        }

        [Test]
        public void ToString_ReturnsFullName()
        {
            var name = FullName.Create("Jane", "Doe");
            Assert.That(name.ToString(), Is.EqualTo("Jane Doe"));
        }

        [Test]
        public void Equality_SameValues_AreEqual()
        {
            var a = FullName.Create("John", "Doe");
            var b = FullName.Create("John", "Doe");
            Assert.That(a, Is.EqualTo(b));
        }
    }
}
