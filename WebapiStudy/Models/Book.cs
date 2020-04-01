using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebapiStudy.Models
{
  public class Book
  {

    public Book(int id,string name,double price,int rating,string desc, BookCategory[] categories)
    {
      this.BookName = name;
      this.BookId = id;
      this.BookPrice = price;
      this.BookDesc = desc;
      this.BookRating = rating;
      this.BookCategories = categories;
    }
    public int BookId { get; set; }
    public string BookName { get; set; }
    public double BookPrice { get; set; }
    public int BookRating { get; set; }
    public string BookDesc { get; set; }
    public IEnumerable<BookCategory> BookCategories { get; set; }
  }
}