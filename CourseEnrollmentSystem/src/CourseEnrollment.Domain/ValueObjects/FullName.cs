using System;
using System.Collections.Generic;
using System.Text;
using CourseEnrollment.Domain.Exceptions;

namespace CourseEnrollment.Domain.ValueObjects
{
    public sealed class FullName : IEquatable<FullName>
    {
        private FullName(string fistName, string lastName)
        {
            FistName = fistName;
            LastName = lastName;
        }

        public string FistName { get; }
        public string LastName { get; }

        public static FullName Create(string fistName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(fistName))
                throw new DomainException("First name is required.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name is required.");
            return new FullName(fistName.Trim(), lastName.Trim());
        }

        public override string ToString()
        {
            return $"{FistName} {LastName}";
        }

        public override bool Equals(object? obj)
        
            =>  obj is FullName other && 
                FistName == other.FistName && 
                LastName == other.LastName;
        
        public bool Equals(FullName? other)
            =>  other is not null && 
                FistName == other.FistName &&
                LastName == other.LastName;

        public override int GetHashCode()
            => HashCode.Combine(FistName, LastName);
    }
}
