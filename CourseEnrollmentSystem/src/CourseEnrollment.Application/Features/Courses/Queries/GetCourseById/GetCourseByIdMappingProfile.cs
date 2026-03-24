using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Enums;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourseById
{
    public class GetCourseByIdMappingProfile : Profile
    {
        public GetCourseByIdMappingProfile()
        {
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.ActiveEnrollments,
                    opt => opt.MapFrom(src =>
                        src.Enrollments.Count(e => 
                            e.Status == EnrollmentStatus.Active)));
        }
    }

}
