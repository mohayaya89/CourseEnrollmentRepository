using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CourseEnrollment.Domain.Exceptions;

namespace CourseEnrollment.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string value) 
        { 
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email cannot be empty");

            if (!IsValidEmail(value))
                throw new DomainException("Invalid email format");

            return new Email(value);
        }

        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        public override bool Equals(object? obj) 
            => obj is Email other && Value == other.Value;

        public bool Equals(Email? other) 
            => other is not null && Value == other.Value;

        public override int GetHashCode() 
            => Value.GetHashCode();

        public override string ToString()
            => Value;

        public static implicit operator string(Email email)
            => email.Value;
    }
}
