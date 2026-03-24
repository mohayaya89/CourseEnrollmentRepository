using System;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}
