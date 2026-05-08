namespace CourseEnrollment.Application.Common.Auth
{
    /// <summary>Represents a success/failure outcome with an optional collection of error messages.</summary>
    public class Result
    {
        /// <summary>Whether the operation succeeded.</summary>
        public bool Succeeded { get; protected init; }

        /// <summary>Error messages describing why the operation failed. Empty on success.</summary>
        public IReadOnlyList<string> Errors { get; protected init; } = [];

        /// <summary>Creates a successful result.</summary>
        public static Result Success() => new() { Succeeded = true };

        /// <summary>Creates a failed result with one or more error messages.</summary>
        public static Result Failure(params string[] errors) => new() { Succeeded = false, Errors = errors };
    }
}
