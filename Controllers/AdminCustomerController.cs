using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminCustomerController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: AdminCustomer
        public ActionResult Index()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Customers";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        CustomerID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        PasswordHash = reader.GetString(4),
                        CreatedAt = reader.GetDateTime(5)
                    });
                }
            }

            return View(customers);
        }

        // GET: AdminCustomer/Details/5
        public ActionResult Details(int id)
        {
            Customer customer = GetCustomerById(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: AdminCustomer/Delete/5
        public ActionResult Delete(int id)
        {
            Customer customer = GetCustomerById(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: AdminCustomer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Delete customer from the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // Helper method to get customer by ID
        private Customer GetCustomerById(int id)
        {
            Customer customer = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@CustomerID", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    customer = new Customer
                    {
                        CustomerID = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        PasswordHash = reader.GetString(4),
                        CreatedAt = reader.GetDateTime(5)
                    };
                }
            }
            return customer;
        }
    }
}
