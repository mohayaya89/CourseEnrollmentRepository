using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Exceptions;

namespace CourseEnrollment.Domain.ValueObjects
{
    public sealed class FullName : IEquatable<FullName>
    {
        private FullName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string LastName { get; }

        public static FullName Create(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name is required.");
            return new FullName(firstName.Trim(), lastName.Trim());
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public override bool Equals(object? obj)

            =>  obj is FullName other &&
                FirstName == other.FirstName &&
                LastName == other.LastName;

        public bool Equals(FullName? other)
            =>  other is not null &&
                FirstName == other.FirstName &&
                LastName == other.LastName;

        public override int GetHashCode()
            => HashCode.Combine(FirstName, LastName);
    }
}
