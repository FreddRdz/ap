using ap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace ap.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _connectionString;
        public AdminController()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }
        // GET: Admin
        public ActionResult Index()
        {
            List<Administrador> admins = new List<Administrador>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerTodosLosAdmins", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        admins.Add(new Administrador
                        {
                            IdAdmin = (int)reader["IdAdmin"],
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString()
                        });
                    }
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(admins);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(Administrador admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = new Usuario();
                    admin.Contrasenia = usuario.EncryptPasswordMD5(admin.Contrasenia);

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand("sp_InsertarAdmin", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.AddWithValue("@Nombre", admin.Nombre);
                        command.Parameters.AddWithValue("@Correo", admin.Correo);
                        command.Parameters.AddWithValue("@Contrasenia", admin.Contrasenia);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Administrador creado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error al crear el administrador: " + ex.Message;
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Hay errores en los datos del formulario. Por favor corrígelos.";
            }

            return View(admin);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            Administrador admin = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerAdminPorId", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdAdmin", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        admin = new Administrador
                        {
                            IdAdmin = (int)reader["IdAdmin"],
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString()
                        };
                    }
                }

                if (admin == null)
                {
                    TempData["ErrorMessage"] = "El administrador no fue encontrado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar los datos del administrador: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(Administrador admin)
        {
            ModelState.Remove("Contrasenia"); // Remover validación de contraseña si no se está editando

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand("sp_ActualizarAdmin", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.AddWithValue("@IdAdmin", admin.IdAdmin);
                        command.Parameters.AddWithValue("@Nombre", admin.Nombre);
                        command.Parameters.AddWithValue("@Correo", admin.Correo);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Administrador actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error al actualizar el administrador: " + ex.Message;
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Hay errores en los datos del formulario. Por favor corrígelos.";
            }

            return View(admin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            Administrador admin = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerAdminPorId", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdAdmin", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        admin = new Administrador
                        {
                            IdAdmin = (int)reader["IdAdmin"],
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString()
                        };
                    }
                }

                if (admin == null)
                {
                    TempData["ErrorMessage"] = "El administrador no fue encontrado.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar los datos del administrador: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_EliminarAdmin", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdAdmin", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Administrador eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el administrador: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}