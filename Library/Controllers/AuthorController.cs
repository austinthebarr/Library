using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Library.Models;

namespace Libary.Controllers
{
  public class AuthorsController : Controller
  {
    [HttpGet("/author/add")]
    public ActionResult AuthorForm()
    {
      return View();
    }
    [HttpPost("/author/add")]
    public ActionResult CollectAuthor(string newauthor)
    {
      Author newAuthor = new Author(newauthor);
      newAuthor.Save();
      return RedirectToAction("AuthorList");
    }
    [HttpGet("/author/list")]
    public ActionResult AuthorList()
    {
      return View(Author.GetAllAuthors());
    }
  }
}
