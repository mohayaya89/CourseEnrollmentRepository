using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CourseEnrollment.Application.Common.Exceptions;
using CourseEnrollment.Application.Features.Courses.Queries.GetCourseById;
using CourseEnrollment.Domain.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourses
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, IEnumerable<CourseListDto>>
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;

        public GetCoursesQueryHandler(
            ICourseRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseListDto>> Handle(
            GetCoursesQuery request,
            CancellationToken ct)
        {
            var course = await _repository.GetAllAsync(ct);

            return _mapper.Map<IEnumerable<CourseListDto>>(course);
        }
    }
}
