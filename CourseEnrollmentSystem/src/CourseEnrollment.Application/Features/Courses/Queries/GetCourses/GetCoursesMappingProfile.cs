using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CourseEnrollment.Domain.Entities;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourses
{
    public class GetCoursesMappingProfile : Profile
    {
        public GetCoursesMappingProfile()
        {
            CreateMap<Course, CourseListDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
        
    }
}
