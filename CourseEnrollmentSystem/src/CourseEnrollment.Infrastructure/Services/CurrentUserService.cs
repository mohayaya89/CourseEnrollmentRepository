using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using CourseEnrollment.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseEnrollment.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                return string.IsNullOrEmpty(userId)
                    ? throw new Exception("User not authenticated")
                    : Guid.Parse(userId);
                
            }
        }
    }
}
