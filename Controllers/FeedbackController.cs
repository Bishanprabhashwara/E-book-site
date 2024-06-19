using System;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: Feedback/Feedback
        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }

        // POST: Feedback/Feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(Feedback model)
        {
            if (ModelState.IsValid)
            {
                SaveFeedbackToDatabase(model);
                return RedirectToAction("FeedbackSubmitted"); // Redirect to feedback submitted page
            }
            return View(model);
        }

        // Method to save feedback data to the database
        private void SaveFeedbackToDatabase(Feedback model)
        {
            string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO Feedback (CustomerID, BookID, Rating, Comment, CreatedAt) " +
                                 "VALUES (@CustomerID, @BookID, @Rating, @Comment, @CreatedAt)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", model.CustomerID);
                        command.Parameters.AddWithValue("@BookID", model.BookID);
                        command.Parameters.AddWithValue("@Rating", model.Rating);
                        command.Parameters.AddWithValue("@Comment", model.Comment);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
                
            }
            catch
            {
                
            }
        }

        // GET: Feedback/FeedbackSubmitted
        public ActionResult FeedbackSubmitted()
        {
            return View();
        }
    }
}
