using System;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedOn { get; protected set; }
        public string? CreatedBy { get; protected set; }

        public DateTime? ModifiedOn { get; protected set; }
        public string? ModifiedBy { get; protected set; }
    }

}
