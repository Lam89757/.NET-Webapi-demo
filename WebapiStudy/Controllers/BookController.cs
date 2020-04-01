using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebapiStudy.Models;
using System.Web.Http.Cors;
using System.IO;
using System.Threading;

namespace WebapiStudy.Controllers
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
  public class BookController : ApiController
  {
    private static object locker = new object();
    private static readonly List<StreamWriter> Subscriber = new List<StreamWriter>();

    List<Book> books = new List<Book>
    {
      new Book(1, "Android book1", 23.24, 4, "a book for android", new BookCategory[]{new BookCategory("IT",true),new BookCategory("金融",true),new BookCategory("互联网",false) }),
      new Book(2, "Java book2", 25.56, 3, "a book for Java", new BookCategory[]{new BookCategory("IT",true),new BookCategory("金融",true),new BookCategory("互联网",false) }),
      new Book(3, "Typescript book3", 30.78, 4, "a book for Typescript", new BookCategory[]{new BookCategory("IT", true),new BookCategory("金融",true),new BookCategory("互联网",false) }),
      new Book(4, "C# book4", 43.50, 1, "a book for C#", new BookCategory[]{new BookCategory("IT",true),new BookCategory("金融",true),new BookCategory("互联网",false) }),
      new Book(5, "C++ book5", 12.43, 2, "a book for C++", new BookCategory[]{new BookCategory("IT", true),new BookCategory("金融",true),new BookCategory("互联网",false) }),
      new Book(6, "ASP.NET book6", 65.33, 5, "a book for ASP.NET", new BookCategory[]{new BookCategory("IT",true),new BookCategory("金融", false),new BookCategory("互联网",false) }),
      new Book(7, "Testing book7", 43.23, 2, "a book for Testing", new BookCategory[]{new BookCategory("IT", false),new BookCategory("金融",true),new BookCategory("互联网",false) }),
      new Book(8, "Javascript book8", 20.99, 3, "a book for Javascript", new BookCategory[]{new BookCategory("IT",true),new BookCategory("金融",true),new BookCategory("互联网", true) }),
    };

    [Route("api/sse")]
    [HttpGet]
    public HttpResponseMessage Get(HttpRequestMessage request)
    {
      var response = new HttpResponseMessage
      {
        Content = new PushStreamContent(async (respStream, content, context) =>
        {
          var subscriber = new StreamWriter(respStream)
          {
            AutoFlush = true
          };
          lock (locker)
          {
            Subscriber.Add(subscriber);
          }
          int i = 0;
          while (1 == 1)
          {
            i++;
            Thread.Sleep(3000);
            await subscriber.WriteLineAsync("data:" + i.ToString() + "\n");
          }
        }, "text/event-stream")

      };
      return response;

    }

    //1.用于获取全部图书列表（get）
    [Route("api/book")]  //可以根据 http://url地址:port/api/book 可以访问
    [HttpGet] //定义动作
    public IEnumerable<Book> getBook()
    {
      return books;
    }
    //2.用户按照id获取图书信息(get)
    [Route("api/book/{id:int}")] //动态传入的参数用{}，可以加约束int
    [HttpGet]
    public Book GetBook(int id)
    {
      return books.Find(book => book.BookId == id);
    }
    //3.用于按关键字搜索图书信息(get)
    [Route("api/book/{name}")] //参数名字一定要对应上
    [HttpGet]
    public IEnumerable<Book> getBooksByName(string name)
    {
      List<Book> searchBooks = new List<Book>();
      books.ForEach(book =>
      {
        if (book.BookName.ToUpper().Contains(name.ToUpper())) //不区分大小写
        {
          searchBooks.Add(book);
        }
      });
      return searchBooks;
    }
    //4.创建一本新的图书(post)
    [Route("api/book/add")]
    [HttpPost]
    public Book addBook(Book book)
    {
      books.Add(book);
      return book;
    }
  }
}
