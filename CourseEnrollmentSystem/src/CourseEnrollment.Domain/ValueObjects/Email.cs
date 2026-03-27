using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using CourseEnrollment.Domain.Exceptions;

namespace CourseEnrollment.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        public string Value { get; } = string.Empty;

        private Email() { } // EF materialization helper

        private Email(string value)
        {
            // normalize: trim and fold to lower-case for consistent equality/hash semantics
            Value = value.Trim().ToLowerInvariant();
        }

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email cannot be empty");

            var normalized = value.Trim();

            if (!IsValidEmail(normalized))
                throw new DomainException("Invalid email format");

            return new Email(normalized);
        }

        private static bool IsValidEmail(string email)
        {
            // Basic regex check first (fast fail), then try MailAddress for additional validation.
            if (!Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase))
            {
                return false;
            }

            try
            {
                // MailAddress accepts a wide range but will catch many malformed addresses.
                var _ = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public override bool Equals(object? obj)
            => obj is Email other && string.Equals(Value, other.Value, StringComparison.Ordinal);

        public bool Equals(Email? other)
            => other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override int GetHashCode()
            => StringComparer.Ordinal.GetHashCode(Value);

        public override string ToString()
            => Value;

        public static implicit operator string(Email email)
            => email.Value;

        public static bool operator ==(Email? left, Email? right)
            => Equals(left, right);

        public static bool operator !=(Email? left, Email? right)
            => !Equals(left, right);
    }
}
