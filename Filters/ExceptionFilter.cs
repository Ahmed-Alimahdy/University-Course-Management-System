using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

public class DbExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is DbUpdateException)
        {
            
            var studentId = context.RouteData.Values["StudentID"]; 
            context.Result = new RedirectToActionResult("AssignCourseToStudent", "Student",
                new { id = studentId });

            context.HttpContext.Items["ErrorMessage"] = "This course is already assigned to the student!";

            context.ExceptionHandled = true;
        }
    }
}
