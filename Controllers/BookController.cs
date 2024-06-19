using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BookController : Controller
    {
        private string connectionString = "Data Source=DESKTOP-CJT3444\\SQLEXPRESS;Initial Catalog=ebook;Integrated Security=True";

        // GET: Book/Search
        public ActionResult Search(string searchTerm)
        {
            var model = new BookSearchViewModel();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                model.SearchTerm = searchTerm;
                model.Results = SearchBooks(searchTerm);
            }

            return View(model);
        }

        private List<Book> SearchBooks(string searchTerm)
        {
            var books = new List<Book>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Books WHERE Title LIKE @SearchTerm OR Author LIKE @SearchTerm";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var book = new Book
                            {
                                BookID = Convert.ToInt32(reader["BookID"]),
                                Title = reader["Title"].ToString(),
                                Author = reader["Author"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToDecimal(reader["Price"]),
                                Category = reader["Category"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                            };

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }
    }
}
