using CourseEnrollment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseEnrollment.Domain.Entities
{
    public class Instructor
    {
        public int EmployeeNumber { get; set; }
        public InstructorTitle Title { get; set; }
        public int DepartmentId { get; set; }
        public DateTime? HireDate { get; set; }
    }
}
