using Microsoft.AspNetCore.Identity;

namespace CourseEnrollment.Infrastructure.Identity
{
    /// <summary>
    /// Identity user entity. Lives in Infrastructure so the Domain and Application
    /// layers remain free of Identity framework types.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary>FK to the domain <c>Student</c> entity, set once during registration.</summary>
        public Guid? StudentId { get; set; }
    }
}
