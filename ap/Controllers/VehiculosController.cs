using ap.Models;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace ap.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly string _connectionString;

        public VehiculosController()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        }

        // GET: Vehiculos
        public ActionResult AddVehiculo(int idCliente)
        {
            // Pasar el ID del cliente para asociarlo con el vehículo
            var vehiculo = new Vehiculo
            {
                IdCliente = idCliente  // Este campo asociará el vehículo con el cliente
            };

            return View(vehiculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVehiculo(Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand("sp_AgregarVehiculo", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        command.Parameters.AddWithValue("@IdCliente", vehiculo.IdCliente);
                        command.Parameters.AddWithValue("@Marca", vehiculo.Marca);
                        command.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
                        command.Parameters.AddWithValue("@Anio", vehiculo.Anio);
                        command.Parameters.AddWithValue("@Placa", vehiculo.Placa);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    TempData["SuccessMessage"] = "Vehiculo creado exitosamente.";

                    return RedirectToAction("Index", "Cliente");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error al agregar el vehículo: " + ex.Message;
                }
            }

            TempData["ErrorMessage"] = "Faltan algunos campos por enviar.";

            return View(vehiculo);
        }
    }
}