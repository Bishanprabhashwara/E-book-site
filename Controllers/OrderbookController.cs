using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrderbookController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: Orderbook
        public ActionResult Index()
        {
            List<Orderbook> orderbooks = new List<Orderbook>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT OrderID, CustomerID, OrderDate, TotalAmount FROM Orderbook";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Orderbook orderbook = new Orderbook
                    {
                        OrderID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        OrderDate = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3)
                    };
                    orderbooks.Add(orderbook);
                }
            }

            return View(orderbooks);
        }

        // GET: Orderbook/Details/5
        public ActionResult Details(int id)
        {
            Orderbook orderbook = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT OrderID, CustomerID, OrderDate, TotalAmount FROM Orderbook WHERE OrderID = @OrderID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderID", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    orderbook = new Orderbook
                    {
                        OrderID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        OrderDate = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3)
                    };
                }
            }

            if (orderbook == null)
            {
                return HttpNotFound();
            }

            return View(orderbook);
        }

        // GET: Orderbook/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orderbook/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orderbook orderbook)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO Orderbook (CustomerID, OrderDate, TotalAmount) VALUES (@CustomerID, @OrderDate, @TotalAmount)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@CustomerID", orderbook.CustomerID);
                    command.Parameters.AddWithValue("@OrderDate", orderbook.OrderDate);
                    command.Parameters.AddWithValue("@TotalAmount", orderbook.TotalAmount);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(orderbook);
        }

        // GET: Orderbook/Edit/5
        public ActionResult Edit(int id)
        {
            Orderbook orderbook = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT OrderID, CustomerID, OrderDate, TotalAmount FROM Orderbook WHERE OrderID = @OrderID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderID", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    orderbook = new Orderbook
                    {
                        OrderID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        OrderDate = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3)
                    };
                }
            }

            if (orderbook == null)
            {
                return HttpNotFound();
            }

            return View(orderbook);
        }

        // POST: Orderbook/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orderbook orderbook)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "UPDATE Orderbook SET CustomerID = @CustomerID, OrderDate = @OrderDate, TotalAmount = @TotalAmount WHERE OrderID = @OrderID";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@OrderID", orderbook.OrderID);
                    command.Parameters.AddWithValue("@CustomerID", orderbook.CustomerID);
                    command.Parameters.AddWithValue("@OrderDate", orderbook.OrderDate);
                    command.Parameters.AddWithValue("@TotalAmount", orderbook.TotalAmount);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(orderbook);
        }

        // GET: Orderbook/Delete/5
        public ActionResult Delete(int id)
        {
            Orderbook orderbook = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT OrderID, CustomerID, OrderDate, TotalAmount FROM Orderbook WHERE OrderID = @OrderID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderID", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    orderbook = new Orderbook
                    {
                        OrderID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        OrderDate = reader.GetDateTime(2),
                        TotalAmount = reader.GetDecimal(3)
                    };
                }
            }

            if (orderbook == null)
            {
                return HttpNotFound();
            }

            return View(orderbook);
        }

        // POST: Orderbook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Orderbook WHERE OrderID = @OrderID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@OrderID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
