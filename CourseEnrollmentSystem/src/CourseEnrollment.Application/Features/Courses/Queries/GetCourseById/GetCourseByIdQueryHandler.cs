using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CourseEnrollment.Application.Common.Exceptions;
using CourseEnrollment.Domain.Enums;
using CourseEnrollment.Domain.Interfaces;
using MediatR;

namespace CourseEnrollment.Application.Features.Courses.Queries.GetCourseById
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
    {
        private readonly ICourseRepository _repository;
        private readonly IMapper _mapper;

        public GetCourseByIdQueryHandler(
            ICourseRepository repository,
            IMapper mapper)
        {

            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CourseDto> Handle(
            GetCourseByIdQuery request,
            CancellationToken ct)
        {
            var course = await _repository.GetByIdAsync(request.Id, ct);

            if (course is null)
                throw new NotFoundException("Course not found");

            return _mapper.Map<CourseDto>(course);
        }
    }



}
