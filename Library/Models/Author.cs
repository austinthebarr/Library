using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;
using System;

namespace Library.Models
{
  public class Author
  {
    private int _id;
    private string _name;

    public Author(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = (this.GetId() == newAuthor.GetId());
        bool nameEquality = (this.GetName() == newAuthor.GetName());
        return (idEquality && nameEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd  = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors (name) VALUES (@AuthorName);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@AuthorName";
      name.Value = _name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public static List<Author> GetAllAuthors()
    {
      List<Author> allAuthors = new List<Author> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM authors;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int AuthorId = rdr.GetInt32(0);
        string AuthorName = "";
        if(!rdr.IsDBNull(1))
        {
          AuthorName = rdr.GetString(1);
        }
        Author newAuthor = new Author(AuthorName, AuthorId);
        allAuthors.Add(newAuthor);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allAuthors;
    }

    public static Author FindAuthor(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM authors WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int AuthorId = 0;
      string AuthorName = "";

      while(rdr.Read())
      {
        AuthorId = rdr.GetInt32(0);
        AuthorName = rdr.GetString(1);
      }

      Author foundAuthor = new Author(AuthorName, AuthorId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundAuthor;
    }

    public void AddBook(Book newBook)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);";

      MySqlParameter author_id = new MySqlParameter();
      author_id.ParameterName = "@AuthorId";
      author_id.Value = _id;
      cmd.Parameters.Add(author_id);

      MySqlParameter book_id = new MySqlParameter();
      book_id.ParameterName = "@BookId";
      book_id.Value = newBook.GetId();
      cmd.Parameters.Add(book_id);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Book> GetBooks()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT books.* FROM authors
      JOIN authors_books ON (authors.id = authors_books.author_id)
      JOIN books ON (authors_books.book_id = books.id)
      WHERE authors.id = @AuthorId;";

      MySqlParameter author_id = new MySqlParameter();
      author_id.ParameterName = "@AuthorId";
      author_id.Value = _id;
      cmd.Parameters.Add(author_id);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Book> books = new List<Book> {};
      while(rdr.Read())
        {
          int bookId = rdr.GetInt32(0);
          string bookTitle = rdr.GetString(1);
          string bookGenre = rdr.GetString(2);
          int bookInventory = rdr.GetInt32(3);
          Book newBook = new Book(bookTitle, bookGenre, bookInventory, bookId);
          books.Add(newBook);
        }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return books;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM authors;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
