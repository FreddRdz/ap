using ap.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using ap.Atributos;

namespace ap.Controllers
{
    public class CitasController : Controller
    {
        private readonly string _connectionString;
        public CitasController()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }

        // GET: Citas
        [AuthSession]
        public ActionResult Index()
        {
            List<Cita> citas = new List<Cita>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerCitas", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Cita cita = new Cita
                        {
                            IdCita = (int)reader["IdCita"],
                            IdVehiculo = (int)reader["IdVehiculo"],
                            IdCliente = (int)reader["IdCliente"],
                            FechaInicio = (DateTime)reader["FechaInicio"],
                            FechaTerminacion = reader["FechaTerminacion"] != DBNull.Value ? (DateTime?)reader["FechaTerminacion"] : null,
                            Comentarios = reader["Comentarios"] != DBNull.Value ? (string)reader["Comentarios"] : null,
                            Estado = reader["Estado"] != DBNull.Value ? (bool?)reader["Estado"] : null,
                            Vehiculo = reader["Vehiculo"].ToString(),
                        };

                        citas.Add(cita);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener las citas: " + ex.Message;
            }

            return View(citas);
        }

        [AdminAuthorize]
        public ActionResult Create(int idCliente)
        {
            try
            {
                Cliente cliente = ObtenerInformacionCliente(idCliente);

                if (cliente == null)
                {
                    TempData["ErrorMessage"] = "Cliente no encontrado.";
                    return RedirectToAction("Index", "Clientes");
                }

                var cita = new Cita
                {
                    IdCliente = idCliente
                };

                ViewBag.Cliente = cliente;

                ViewBag.Vehiculos = ObtenerVehiculosPorCliente(idCliente);
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
                TempData["ErrorMessage"] = "Error al cargar el formulario de cita: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View(new Cita());
        }

        // POST: Citas/Create - Crear una nueva cita
        [HttpPost]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cita cita)
        {
            ModelState.Remove("FechaFin");
            ModelState.Remove("Estado");

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand("sp_CrearCita", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        command.Parameters.AddWithValue("@IdVehiculo", cita.IdVehiculo);
                        command.Parameters.AddWithValue("@FechaInicio", cita.FechaInicio);
                        command.Parameters.AddWithValue("@IdCliente", cita.IdCliente);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Cita creada exitosamente.";

                    return RedirectToAction("Index");
                }
                catch (SqlException ex)
                {
                    // Manejar el error lanzado por el SP, código de error 50000 indica nuestro THROW
                    if (ex.Number == 51000)
                    {
                        TempData["ErrorMessage"] = "Error al crear la cita: " + ex.Message;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al crear la cita: " + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error al crear la cita: " + ex.Message;
                }
            }

            // Volver a obtener la lista de vehículos si hay un error en el formulario

            Cliente cliente = ObtenerInformacionCliente(cita.IdCliente);

            if (cliente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToAction("Index", "Clientes");
            }

            ViewBag.Cliente = cliente;

            ViewBag.Vehiculos = ObtenerVehiculosPorCliente(cita.IdCliente);
            return View(cita);
        }

        // GET: Citas/Edit/{id} - Mostrar el formulario para editar una cita
        [AdminAuthorize]
        public ActionResult Edit(int id)
        {
            Cita cita = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerCitaPorId", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdCita", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cita = new Cita
                        {
                            IdCita = (int)reader["IdCita"],
                            IdVehiculo = (int)reader["IdVehiculo"],
                            FechaInicio = (DateTime)reader["FechaInicio"],
                            IdCliente = (int)reader["IdCliente"],
                        };
                    }
                }

                if (cita == null)
                {
                    return HttpNotFound();
                }

                var cliente = ObtenerInformacionCliente(cita.IdCliente);

                // Obtener la lista de vehículos del cliente
                ViewBag.Vehiculos = ObtenerVehiculosPorCliente(cita.IdCliente);
                ViewBag.Cliente = cliente;
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
                TempData["ErrorMessage"] = "Error al cargar los datos de la cita: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View(cita);
        }

        // POST: Citas/Edit/{id} - Guardar los cambios en una cita
        [HttpPost]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cita cita)
        {

            ModelState.Remove("FechaFin");
            ModelState.Remove("Estado");

            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand("sp_ActualizarCita", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        command.Parameters.AddWithValue("@IdCita", cita.IdCita);
                        command.Parameters.AddWithValue("@IdVehiculo", cita.IdVehiculo);
                        command.Parameters.AddWithValue("@FechaInicio", cita.FechaInicio);
                        command.Parameters.AddWithValue("@FechaFin", cita.FechaInicio.AddHours(2));  // Actualizar FechaFin también

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Cita actualizada exitosamente.";
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
                    TempData["ErrorMessage"] = "Error al actualizar la cita: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Valide sus campos.";
            }

            // Volver a obtener la lista de vehículos si hay un error en el formulario
            ViewBag.Vehiculos = ObtenerVehiculosPorCliente(cita.IdCliente);
            return View(cita);
        }

        // GET: Citas/Delete/{id} - Confirmar eliminación de una cita
        [AdminAuthorize]
        public ActionResult Delete(int id)
        {
            Cita cita = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerCitaPorId", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdCita", id);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        cita = new Cita
                        {
                            IdCita = (int)reader["IdCita"],
                            IdVehiculo = (int)reader["IdVehiculo"],
                            FechaInicio = (DateTime)reader["FechaInicio"],
                            FechaFin = (DateTime)reader["FechaFin"],
                            Estado = (bool)reader["Estado"],
                            Comentarios = reader["ComentariosAdmin"] != DBNull.Value ? (string)reader["ComentariosAdmin"] : null,
                        };
                    }
                }

                if (cita == null)
                {
                    return HttpNotFound();
                }
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
                TempData["ErrorMessage"] = "Error al cargar los datos de la cita: " + ex.Message;
                return RedirectToAction("Index");
            }

            return View(cita);
        }

        // POST: Citas/Delete/{id} - Eliminar una cita
        [HttpPost, ActionName("Delete")]
        [AdminAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_EliminarCita", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdCita", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

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
                TempData["ErrorMessage"] = "Error al eliminar la cita: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AdminAuthorize]
        public ActionResult AprobarCita(int idCita, string comentarios, DateTime fechaTerminacion)
        {
            string userEmail = Session["UserEmail"] as string;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ActualizarEstadoCita", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@IdCita", idCita);
                    command.Parameters.AddWithValue("@Estado", true);  // Aprobado
                    command.Parameters.AddWithValue("@Correo", userEmail);
                    command.Parameters.AddWithValue("@FechaTerminacion", fechaTerminacion);
                    command.Parameters.AddWithValue("@Comentarios", comentarios ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [AdminAuthorize]
        public ActionResult DesaprobarCita(int idCita, string comentarios)
        {
            string userEmail = Session["UserEmail"] as string;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ActualizarEstadoCita", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@IdCita", idCita);
                    command.Parameters.AddWithValue("@Correo", userEmail);
                    command.Parameters.AddWithValue("@Estado", false);  // Desaprobado
                    command.Parameters.AddWithValue("@Comentarios", comentarios ?? (object)DBNull.Value);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Método para obtener la lista de vehículos por cliente
        private List<SelectListItem> ObtenerVehiculosPorCliente(int idCliente)
        {
            List<SelectListItem> vehiculos = new List<SelectListItem>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand("sp_ObtenerVehiculosPorCliente", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        vehiculos.Add(new SelectListItem
                        {
                            Value = reader["IdVehiculo"].ToString(),
                            Text = reader["Marca"].ToString() + " " + reader["Modelo"].ToString() + " (" + reader["Placa"].ToString() + ")"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener los vehículos del cliente: " + ex.Message;
            }

            return vehiculos;
        }

        private Cliente ObtenerInformacionCliente(int idCliente)
        {
            Cliente cliente = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("sp_ObtenerClientePorId", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@IdCliente", idCliente);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    cliente = new Cliente
                    {
                        IdCliente = (int)reader["IdCliente"],
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        CorreoElectronico = reader["CorreoElectronico"].ToString()
                    };
                }
            }

            return cliente;
        }
    }
}