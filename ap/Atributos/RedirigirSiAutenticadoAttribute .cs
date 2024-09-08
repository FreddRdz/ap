using System.Web;
using System.Web.Mvc;

namespace ap.Atributos
{
    public class RedirigirSiAutenticadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = HttpContext.Current.Session;
            var request = HttpContext.Current.Request;

            // Verificar si el usuario ya tiene sesión activa
            if (session["UserEmail"] != null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Home", action = "Index" }
                    )
                );
                return;
            }

            // Verificar si existe una cookie de autenticación
            HttpCookie authCookie = request.Cookies["UserCookie"];
            if (authCookie != null)
            {
                // Restaurar la sesión desde la cookie
                session["UserEmail"] = authCookie.Values["UserEmail"];
                session["UserRole"] = authCookie.Values["UserRole"];

                // Redirigir al Dashboard/Home si la sesión ha sido restaurada
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Home", action = "Index" }
                    )
                );
            }

            base.OnActionExecuting(filterContext); // Continuar con la ejecución de la acción
        }
    }
}