using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: Admin/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Admin/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Admin model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.PasswordHash))
                {
                    model.PasswordHash = ComputeSha256Hash(model.PasswordHash); // Hash the password
                }
                model.CreatedAt = DateTime.Now;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO Admins (Username, PasswordHash, CreatedAt) VALUES (@Username, @PasswordHash, @CreatedAt)";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Username", model.Username);

                    // Check if PasswordHash is not null or empty before adding it as a parameter
                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        command.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);
                    }
                    else
                    {
                        // Handle the case where PasswordHash is null or empty
                        ModelState.AddModelError("", "Password cannot be null or empty.");
                        return View(model);
                    }

                    command.Parameters.AddWithValue("@CreatedAt", model.CreatedAt);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: Admin/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Password cannot be empty.");
                return View();
            }

            string passwordHash = ComputeSha256Hash(password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT AdminID, Username FROM Admins WHERE Username = @Username AND PasswordHash = @PasswordHash";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Session["AdminID"] = reader.GetInt32(0);
                    Session["Username"] = reader.GetString(1);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View();
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // Additional stub actions for the new functionalities
        public ActionResult ManageBooks()
        {
            return View();
        }

        public ActionResult ManageCustomers()
        {
            return View();
        }

        public ActionResult ManageOrders()
        {
            return View();
        }

        public ActionResult GenerateReports()
        {
            return View();
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
