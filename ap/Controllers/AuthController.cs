using ap.Atributos;
using ap.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace ap.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        [RedirigirSiAutenticado]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [RedirigirSiAutenticado]
        public ActionResult SignIn(string correo, string password)
        {
            try
            {
                var usuario = new Usuario();

                var admin = usuario.ValidateAdmin(correo, password);

                if (admin != null)
                {
                    Session["UserEmail"] = correo;
                    Session["UserRole"] = "Admin";

                    HttpCookie authCookie = new HttpCookie("UserCookie");
                    authCookie.Values["UserEmail"] = correo;
                    authCookie.Values["UserRole"] = "Admin";
                    authCookie.Expires = DateTime.Now.AddDays(7);  // La cookie expirará en 7 días
                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Home");
                }

                // Si no se encuentra ni administrador ni cliente
                TempData["ErrorMessage"] = "Credenciales incorrectas.";
                return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Limpiar la sesión
            Session.Clear();
            Session.Abandon();

            // Limpiar la cookie
            if (Request.Cookies["UserCookie"] != null)
            {
                HttpCookie cookie = new HttpCookie("UserCookie")
                {
                    Expires = DateTime.Now.AddDays(-1)  // Establecer la cookie para que expire
                };
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Login");
        }
    }
}