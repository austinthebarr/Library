using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Library.Models;

namespace Libary.Controllers
{
  public class BooksController : Controller
  {
    [HttpGet("/book/add")]
    public ActionResult BookForm()
    {
      return View();
    }

    [HttpPost("/book/add")]
    public ActionResult CollectBook(string newbook, string newgenre, int newinventory)
    {
      Book newBook = new Book(newbook, newgenre, newinventory);
      newBook.Save();

      return RedirectToAction("Catalog");
    }
    [HttpGet("/book/catalog")]
    public ActionResult Catalog()
    {
      return View(Book.GetAllBooks());
    }
    [HttpGet("/author/{id}/add")]
    public ActionResult AddAuthor(int id)
    {
      Book thisBook = Book.FindBook(id);
      List<Author> allAuthors = Author.GetAllAuthors();

      return View(allAuthors);
    }
    [HttpPost("/author/{id}/add")]
    public ActionResult CollectAuthorForBook(int id)
    {
      Author newAuthor = new Author(Request.Form["newauthor"]);
      Book thisBook = Book.FindBook(id);
      thisBook.AddAuthor(newAuthor);
      return RedirectToAction("Catalog");
    }
  }
}
