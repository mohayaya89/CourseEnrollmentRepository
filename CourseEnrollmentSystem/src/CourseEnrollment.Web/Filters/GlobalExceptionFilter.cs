using CourseEnrollment.Application.Common.Exceptions;
using CourseEnrollment.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CourseEnrollment.Web.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public GlobalExceptionFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                context.Result = new NotFoundResult();
                context.ExceptionHandled = true;
                return;
            }

            if (context.Exception is DomainException)
            {
                var tempData = _tempDataFactory.GetTempData(context.HttpContext);
                tempData["Error"] = context.Exception.Message;

                var referer = context.HttpContext.Request.Headers.Referer.ToString();
                context.Result = string.IsNullOrEmpty(referer)
                    ? new RedirectToActionResult("Index", "Home", null)
                    : new LocalRedirectResult(referer);

                context.ExceptionHandled = true;
            }
        }
    }
}
