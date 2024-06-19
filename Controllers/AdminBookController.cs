using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminBookController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: AdminBook
        public ActionResult Index()
        {
            List<Book> books = new List<Book>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Books";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        BookID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Description = reader.GetString(3),
                        Price = reader.GetDecimal(4),
                        Category = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    });
                }
            }
            return View(books);
        }

        // GET: AdminBook/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminBook/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(book.Author))
                {
                    book.CreatedAt = DateTime.Now;
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string sql = "INSERT INTO Books (Title, Author, Description, Price, Category, CreatedAt) VALUES (@Title, @Author, @Description, @Price, @Category, @CreatedAt)";
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@Title", book.Title);
                        command.Parameters.AddWithValue("@Author", book.Author);
                        command.Parameters.AddWithValue("@Description", book.Description);
                        command.Parameters.AddWithValue("@Price", book.Price);
                        command.Parameters.AddWithValue("@Category", book.Category);
                        command.Parameters.AddWithValue("@CreatedAt", book.CreatedAt);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Author cannot be null or empty.");
                }
            }

            return View(book);
        }

        // GET: AdminBook/Edit/5
        public ActionResult Edit(int id)
        {
            Book book = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Books WHERE BookID = @BookID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BookID", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    book = new Book
                    {
                        BookID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Description = reader.GetString(3),
                        Price = reader.GetDecimal(4),
                        Category = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    };
                }
            }
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: AdminBook/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "UPDATE Books SET Title = @Title, Author = @Author, Description = @Description, Price = @Price, Category = @Category WHERE BookID = @BookID";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Description", book.Description);
                    command.Parameters.AddWithValue("@Price", book.Price);
                    command.Parameters.AddWithValue("@Category", book.Category);
                    command.Parameters.AddWithValue("@BookID", book.BookID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: AdminBook/Delete/5
        public ActionResult Delete(int id)
        {
            Book book = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Books WHERE BookID = @BookID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BookID", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    book = new Book
                    {
                        BookID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Author = reader.GetString(2),
                        Description = reader.GetString(3),
                        Price = reader.GetDecimal(4),
                        Category = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    };
                }
            }
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: AdminBook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Books WHERE BookID = @BookID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BookID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
