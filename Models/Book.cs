using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderItembook> OrderItems { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }

    public class BookSearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<Book> Results { get; set; }
    }
}
