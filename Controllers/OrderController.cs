using System;
using System.Web.Mvc;
using System.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orderbook model)
        {
            if (ModelState.IsValid)
            {
                // Save the order data to the database
                SaveOrderToDatabase(model);

                // Optionally, you can redirect to another action upon successful creation
                return RedirectToAction("OrderSuccess");
            }

            // If model state is not valid, return to the create form with validation errors
            return View(model);
        }

        // Method to save order data to the database
        private void SaveOrderToDatabase(Orderbook model)
        {
            // Replace this connection string with your actual database connection string
            string connectionString = "YourConnectionStringHere";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Orders (CustomerID, OrderDate, TotalAmount) " +
                             "VALUES (@CustomerID, @OrderDate, @TotalAmount); SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", model.CustomerID);
                    command.Parameters.AddWithValue("@OrderDate", model.OrderDate);
                    command.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);

                    model.OrderID = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        // GET: Order/OrderSuccess
        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}
