using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CustomerController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: Customer/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                SaveCustomerToDatabase(model);
                return RedirectToAction("RegistrationSuccess");
            }

            return View(model);
        }

        private void SaveCustomerToDatabase(CustomerModel model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Customers (FirstName, LastName, Email, PasswordHash, CreatedAt) " +
                             "VALUES (@FirstName, @LastName, @Email, @PasswordHash, @CreatedAt)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", model.FirstName);
                    command.Parameters.AddWithValue("@LastName", model.LastName);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }

        // GET: Customer/RegistrationSuccess
        public ActionResult RegistrationSuccess()
        {
            return View();
        }

        // GET: Customer/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Customer/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = AuthenticateUser(model.Email, model.Password);
                if (customer != null)
                {
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        private CustomerModel AuthenticateUser(string email, string password)
        {
            CustomerModel customer = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Customers WHERE Email = @Email AND PasswordHash = @PasswordHash";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PasswordHash", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            customer = new CustomerModel
                            {
                                CustomerID = (int)reader["CustomerID"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                PasswordHash = reader["PasswordHash"].ToString(),
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };
                        }
                    }
                }
            }

            return customer;
        }

        // GET: Customer/Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
