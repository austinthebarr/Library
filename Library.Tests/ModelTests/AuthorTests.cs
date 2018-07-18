using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Library.Models;
using System;

namespace Library.Tests
{
  [TestClass]
  public class AuthorTests : IDisposable
  {
    public void Dispose()
    {
      Author.DeleteAll();
    }

    public AuthorTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    [TestMethod]
    public void GetAllAuthors_empty()
    {
      //Arrange
      //Act
      int result = Author.GetAllAuthors().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void ComapareTwoAuthors()
    {
      //Arrange //Act
      Author firstAuthor = new Author("Ray Bradbury");
      Author secondAuthor = new Author("Ray Bradbury");

      //Assert
      Assert.AreEqual(firstAuthor, secondAuthor);
    }

    [TestMethod]
    public void ReturnListOfAuthors()
    {
      Author firstAuthor = new Author("Ray Bradbury");
      Author secondAuthor = new Author("Jonathan Taplin");

      //Act
      firstAuthor.Save();secondAuthor.Save();
      List<Author> testlist = Author.GetAllAuthors();
      List<Author> testlist2 = new List<Author> {firstAuthor, secondAuthor};

      //Assert
      CollectionAssert.AreEqual(testlist2, testlist);
    }

    [TestMethod]
    public void FindAuthorById()
    {
      //Act //Assert
      Author newAuthor = new Author("Bill Gates");
      newAuthor.Save();
      Author foundAuthor = Author.FindAuthor(newAuthor.GetId());

      //Assert
      Assert.AreEqual(newAuthor, foundAuthor);
    }
  }
}
