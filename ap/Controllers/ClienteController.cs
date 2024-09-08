using ap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using ap.Atributos;

namespace ap.Controllers
{
    public class ClienteController : Controller
    {
        private readonly string _connectionString;

        public ClienteController()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }

        public ActionResult Index()
        {
            List<Cliente> clientes = new List<Cliente>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerClientes", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente
                        {
                            IdCliente = (int)reader["IdCliente"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            Telefono = reader["Telefono"] != DBNull.Value ? (int?)reader["Telefono"] : null, // Manejar NULL en Telefono
                            Direccion = reader["Direccion"].ToString()
                        };

                        clientes.Add(cliente);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar los clientes: " + ex.Message;
            }

            // Asegurarse de que la lista de clientes esté inicializada (aunque esté vacía)
            return View(clientes);
        }

        // GET: Clientes/Create
        [AdminAuthorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            ModelState.Remove("Contrasenia");

            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = new Usuario();

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand("[dbo].[spCrearNuevoCliente]", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        command.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                        command.Parameters.AddWithValue("@CorreoElectronico", cliente.CorreoElectronico);
                        command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        command.Parameters.AddWithValue("@Direccion", cliente.Direccion);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Cliente creado exitosamente.";

                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 51000)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ocurrió un error inesperado al registrar el cliente.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            ViewBag.ErrorMessage = "Hay errores en los datos del formulario. Por favor corrígelos.";

            return View(cliente);
        }

        // GET: Clientes/Edit/{id}
        [AdminAuthorize]
        public ActionResult Edit(int id)
        {
            Cliente cliente = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerClientePorId", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdCliente", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            IdCliente = (int)reader["IdCliente"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            Telefono = (int)reader["Telefono"],
                            Direccion = reader["Direccion"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar los datos del cliente: " + ex.Message;
                return RedirectToAction("Index");
            }

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Edit/{id}
        [HttpPost]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
            ModelState.Remove("Contrasenia");

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand("sp_ActualizarCliente", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);

                        // Solo enviar los parámetros si tienen valores (no nulos)
                        if (!string.IsNullOrEmpty(cliente.Nombre))
                        {
                            command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Nombre", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(cliente.Apellido))
                        {
                            command.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Apellido", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(cliente.CorreoElectronico))
                        {
                            command.Parameters.AddWithValue("@CorreoElectronico", cliente.CorreoElectronico);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CorreoElectronico", DBNull.Value);
                        }

                        if (cliente.Telefono.HasValue)
                        {
                            command.Parameters.AddWithValue("@Telefono", cliente.Telefono.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Telefono", DBNull.Value);
                        }

                        if (!string.IsNullOrEmpty(cliente.Direccion))
                        {
                            command.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Direccion", DBNull.Value);
                        }

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Cliente actualizado exitosamente.";

                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 51000)
                    {
                        TempData["ErrorMessage"] = ex.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Ocurrió un error inesperado al registrar el cliente.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }

            ViewBag.ErrorMessage = "Hay errores en los datos del formulario. Por favor corrígelos.";

            return View(cliente);
        }

        // GET: Clientes/Delete/{id}
        [AdminAuthorize]
        public ActionResult Delete(int id)
        {
            Cliente cliente = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerClientePorId", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdCliente", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cliente = new Cliente
                        {
                            IdCliente = (int)reader["IdCliente"],
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            Telefono = (int)reader["Telefono"],
                            Direccion = reader["Direccion"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al cargar los datos del cliente para eliminar: " + ex.Message;
                return RedirectToAction("Index");
            }

            if (cliente == null)
            {
                return HttpNotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_EliminarCliente", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdCliente", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el cliente: " + ex.Message;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}