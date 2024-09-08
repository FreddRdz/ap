using System.Web;
using System.Web.Mvc;

namespace ap.Atributos
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var userRole = HttpContext.Current.Session["UserRole"];

            if (userRole == null || userRole.ToString() != "Admin")
            {

                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Index", action = "Home" }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}