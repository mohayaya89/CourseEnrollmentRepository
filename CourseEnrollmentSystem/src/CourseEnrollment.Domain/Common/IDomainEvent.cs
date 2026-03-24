using System;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Domain.Common
{
    public interface IDomainEvent 
    {
        DateTime OccurredOn { get; }
    }
}
