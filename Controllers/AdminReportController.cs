using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
    public class AdminReportController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: AdminReport/OrderReport
        public ActionResult OrderReport()
        {
            List<Orderbook> orders = GetOrderDataFromDatabase();
            return View(orders); // Passing the model to the view
        }

  
        private List<Orderbook> GetOrderDataFromDatabase()
        {
            List<Orderbook> orders = new List<Orderbook>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Orderbook"; 
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Orderbook
                    {
                        OrderID = Convert.ToInt32(reader["OrderID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                    });
                }
            }

            return orders;
        }
    }
}
