using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Library.Models;
using System;

namespace Library.Tests
{
  [TestClass]
  public class BookTests : IDisposable
  {
    public void Dispose()
    {
      Book.DeleteAll();
    }
    public  BookTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    [TestMethod]
    public void HasListThatStartsatZero()
    {
      //Arrange
      //Act
      int result = Book.GetAllBooks().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void ComapringTwoBooks()
    {
      //Arrange//Act
      Book firstBook = new Book("Fahrenheit 451", "dystopian", 6);
      Book secondBook = new Book("Fahrenheit 451", "dystopian", 6);
      //Assert
      Assert.AreEqual(firstBook, secondBook);
    }

    [TestMethod]
    public void GetBackListOfBooks()
    {
      //Arrange
      Book firstBook = new Book("Fahrenheit 451", "dystopian", 6);
      Book secondBook = new Book("Killing Me Softly", "romance", 4);
      firstBook.Save();
      secondBook.Save();

      //Act
      List<Book> testList = Book.GetAllBooks();
      List<Book> testList2 = new List<Book>{firstBook, secondBook};

      //Assert
      CollectionAssert.AreEqual(testList, testList2);
    }

    [TestMethod]
    public void FindBookInListById()
    {
      //Arrange
      Book firstBook = new Book("Fahrenheit 451", "dystopian", 6);
      Book secondBook = new Book("Killing Me Softly", "romance", 4);
      firstBook.Save();
      secondBook.Save();

      //Act
      Book foundBook = Book.FindBook(firstBook.GetId());

      //Arrange
      Assert.AreEqual(foundBook, firstBook);
    }
 }
}
