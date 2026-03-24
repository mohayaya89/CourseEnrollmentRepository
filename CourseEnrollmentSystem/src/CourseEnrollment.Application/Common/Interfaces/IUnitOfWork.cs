using System;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}
