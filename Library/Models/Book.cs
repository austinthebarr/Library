using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;
using System;

namespace Library.Models
{
  public class Book
  {
    private int _id;
    private string _title;
    private string _genre;
    private int _inventory;


    public Book(string title, string genre, int inventory, int id = 0)
    {
      _id = id;
      _title = title;
      _genre = genre;
      _inventory = inventory;
    }
    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = (this.GetId() == newBook.GetId());
        bool titleEquality = (this.GetTitle() == newBook.GetTitle());
        bool genreEquality = (this.GetGenre() == newBook.GetGenre());
        bool inventoryEquality = (this.GetInventory() == newBook.GetInventory());

        return (idEquality && titleEquality && genreEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
      return _title;
    }
    public string GetGenre()
    {
      return _genre;
    }
    public int GetInventory()
    {
      return _inventory;
    }
    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM books;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO books (title, genre, inventory) VALUES (@BookTitle, @BookGenre, @BookInventory);";

      MySqlParameter title = new MySqlParameter();
      title.ParameterName = "@BookTitle";
      title.Value = _title;
      cmd.Parameters.Add(title);

      MySqlParameter genre = new MySqlParameter();
      genre.ParameterName = "@BookGenre";
      genre.Value = _genre;
      cmd.Parameters.Add(genre);

      MySqlParameter inventory = new MySqlParameter();
      inventory.ParameterName = "@BookInventory";
      inventory.Value = _inventory;
      cmd.Parameters.Add(inventory);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Book> GetAllBooks()
    {
      List<Book> allBooks = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int BookId = rdr.GetInt32(0);
        string BookTitle = "";
        if(!rdr.IsDBNull(1))
        {
          BookTitle = rdr.GetString(1);
        }
        string BookGenre = "";
        if(!rdr.IsDBNull(2))
        {
          BookGenre = rdr.GetString(2);
        }
        int BookInventory = rdr.GetInt32(3);

        Book newBook = new Book(BookTitle, BookGenre, BookInventory, BookId);
        allBooks.Add(newBook);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allBooks;
    }

    public static Book FindBook(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM books WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int BookId = 0;
      string BookTitle = "";
      string BookGenre = "";
      int BookInventory = 0;

      while(rdr.Read())
      {
        BookId = rdr.GetInt32(0);
        BookTitle = rdr.GetString(1);
        BookGenre = rdr.GetString(2);
        BookInventory = rdr.GetInt32(3);
      }

      Book foundBook = new Book(BookTitle, BookGenre, BookInventory, BookId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundBook;
    }

    public void AddAuthor(Author newAuthor)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO authors_books (author_id, book_id) VALUES (@AuthorId, @BookId);";

      MySqlParameter author_id = new MySqlParameter();
      author_id.ParameterName = "@AuthorId";
      author_id.Value = newAuthor.GetId();
      cmd.Parameters.Add(author_id);

      MySqlParameter book_id = new MySqlParameter();
      book_id.ParameterName = "@BookId";
      book_id.Value = _id;
      cmd.Parameters.Add(book_id);

      cmd.ExecuteNonQuery();
      conn. Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    
    public List<Author> GetAuthors()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText =@"SELECT authors.* FROM books JOIN authors_books ON (books.id = authors_books.book_id) JOIN authors ON (authors_books.author_id = authors.id) WHERE books.id = @BookId;";

      MySqlParameter bookIdParameter = new MySqlParameter();
      bookIdParameter.ParameterName = "@BookId";
      bookIdParameter.Value = _id;
      cmd.Parameters.Add(bookIdParameter);

      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Author> authors = new List<Author>{};

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        Author newAuthor = new Author(authorName, authorId);
        authors.Add(newAuthor);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return authors;
    }
  }
}
