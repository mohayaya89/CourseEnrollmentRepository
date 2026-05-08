namespace CourseEnrollment.Application.Common.Interfaces
{
    /// <summary>Provides information about the currently authenticated user from the request context.</summary>
    public interface ICurrentUserService
    {
        /// <summary>The authenticated user's Identity id (the <c>sub</c> claim).</summary>
        Guid UserId { get; }

        /// <summary>The authenticated user's email address.</summary>
        string? Email { get; }

        /// <summary>The role names assigned to the current user.</summary>
        IReadOnlyList<string> Roles { get; }

        /// <summary>The domain Student entity id linked to this user. Null for non-student users.</summary>
        Guid? StudentId { get; }
    }
}
