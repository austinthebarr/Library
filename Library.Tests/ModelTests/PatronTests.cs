using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Library.Models;
using System;

namespace Library.Tests
{
  [TestClass]
  public class PatronTests : IDisposable
  {
    public void Dispose()
    {
      Author.DeleteAll();
      Patron.DeleteAll();
    }
    public  PatronTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }

    [TestMethod]
    public void HasListThatStartsatZero()
    {
      //Arrange
      //Act
      int result = Patron.GetAllPatrons().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void ComapringTwoPatrons()
    {
      //Arrange//Act
      Patron firstPatron = new Patron("Nick");
      Patron secondPatron = new Patron("Nick");
      //Assert
      Assert.AreEqual(firstPatron, secondPatron);
    }

    [TestMethod]
    public void GetBackListOfPatrons()
    {
      //Arrange
      Patron firstPatron = new Patron("Mick");
      Patron secondPatron = new Patron("Pat");
      firstPatron.Save();
      secondPatron.Save();

      //Act
      List<Patron> testList = Patron.GetAllPatrons();
      List<Patron> testList2 = new List<Patron>{firstPatron, secondPatron};

      //Assert
      CollectionAssert.AreEqual(testList, testList2);
    }

    [TestMethod]
    public void FindPatronInListById()
    {
      //Arrange
      Patron firstPatron = new Patron("Pat");
      Patron secondPatron = new Patron("Gus");
      firstPatron.Save();
      secondPatron.Save();

      //Act
      Patron foundPatron = Patron.FindPatron(firstPatron.GetId());

      //Arrange
      Assert.AreEqual(foundPatron, firstPatron);
    }

    [TestMethod]
    public void AddABookToAPatron()
    {
      //Arrange
      Patron testPatron = new Patron("Beavis");
      testPatron.Save();
      Book testBook = new Book("A tale of Two Cities", "non-fiction", 2);
      testBook.Save();

      //Act
      testPatron.AddBook(testBook);

      List<Book> result = testPatron.GetBooks();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
  }
}
