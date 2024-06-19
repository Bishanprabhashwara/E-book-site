using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminOrderController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: AdminOrder
        public ActionResult Index()
        {
            List<Orderbook> orders = new List<Orderbook>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Orders";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Orderbook
                    {
                        OrderID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        OrderDate = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3)
                    });
                }
            }

            return View(orders);
        }

        // GET: AdminOrder/Details/5
        public ActionResult Details(int id)
        {
            Orderbook order = GetOrderById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: AdminOrder/Delete/5
        public ActionResult Delete(int id)
        {
            Orderbook order = GetOrderById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: AdminOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Orders WHERE OrderID = @OrderID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        private Orderbook GetOrderById(int id)
        {
            Orderbook order = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Orders WHERE OrderID = @OrderID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderID", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    order = new Orderbook
                    {
                        OrderID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        OrderDate = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3)
                    };
                }
            }
            return order;
        }
    }
}
