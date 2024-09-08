using System.Web;
using System.Web.Mvc;

namespace ap.Atributos
{
    public class AuthSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserEmail"] == null)
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies["UserCookie"];

                if (authCookie != null)
                {
                    // Restaurar la sesión a partir de la cookie
                    HttpContext.Current.Session["UserEmail"] = authCookie.Values["UserEmail"];
                    HttpContext.Current.Session["UserId"] = authCookie.Values["UserId"];
                    HttpContext.Current.Session["UserRole"] = "Admin";  // Supongamos que se conoce el rol por el ID
                }
                else
                {
                    // Si no hay ni sesión ni cookie, redirigir al inicio de sesión
                    filterContext.Result = new RedirectResult("~/Auth/Login");
                }
            }

            base.OnActionExecuting(filterContext); // Continuar con la ejecución de la acción
        }
    }
}