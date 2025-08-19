using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace universityManagementSys.Filters
{
    public class ValidateModelNotEmptyFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ViewResult viewResult)
            {
                if (viewResult.Model == null ||
                    (viewResult.Model is IEnumerable<object> modelList && !modelList.Any()))
                {
                    context.Result = new NotFoundResult();
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           
        }
    }
}
