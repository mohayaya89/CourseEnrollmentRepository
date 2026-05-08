namespace CourseEnrollment.Application.Common.Auth
{
    /// <summary>A <see cref="Result"/> that carries a value on success.</summary>
    /// <typeparam name="T">The type of the value returned on success.</typeparam>
    public sealed class Result<T> : Result
    {
        /// <summary>The returned value. Only meaningful when <see cref="Result.Succeeded"/> is <c>true</c>.</summary>
        public T? Value { get; private init; }

        /// <summary>Creates a successful result carrying <paramref name="value"/>.</summary>
        public static Result<T> Success(T value) => new() { Succeeded = true, Value = value };

        /// <summary>Creates a failed result with one or more error messages.</summary>
        public new static Result<T> Failure(params string[] errors) =>
            new() { Succeeded = false, Errors = errors };
    }
}
