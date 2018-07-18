using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;
using System;

namespace Library.Models
{
  public class Patron
  {
    private int _id;
    private string _name;

    public Patron(string Name, int Id = 0)
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

    public override bool Equals(System.Object otherPatron)
    {
      if (!(otherPatron is Patron))
      {
        return false;
      }
      else
      {
        Patron newPatron = (Patron) otherPatron;
        bool idEquality = (this.GetId() == newPatron.GetId());
        bool nameEquality = (this.GetName() == newPatron.GetName());
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
      cmd.CommandText = @"INSERT INTO patrons (name) VALUES (@PatronName);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@PatronName";
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
    public static List<Patron> GetAllPatrons()
    {
      List<Patron> allPatrons = new List<Patron> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int PatronId = rdr.GetInt32(0);
        string PatronName = "";
        if(!rdr.IsDBNull(1))
        {
          PatronName = rdr.GetString(1);
        }
        Patron newPatron = new Patron(PatronName, PatronId);
        allPatrons.Add(newPatron);
      }
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return allPatrons;
    }

    public static Patron FindPatron(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int PatronId = 0;
      string PatronName = "";

      while(rdr.Read())
      {
        PatronId = rdr.GetInt32(0);
        PatronName = rdr.GetString(1);
      }

      Patron foundPatron = new Patron(PatronName, PatronId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundPatron;
    }

    public void AddBook(Book newBook)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patrons_books (patron_id, book_id) VALUES (@PatronId, @BookId);";

      MySqlParameter patron_id = new MySqlParameter();
      patron_id.ParameterName = "@PatronId";
      patron_id.Value = _id;
      cmd.Parameters.Add(patron_id);

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
      cmd.CommandText = @"SELECT books.* FROM patrons
      JOIN patrons_books ON (patrons.id = patrons_books.patron_id)
      JOIN books ON (patrons_books.book_id = books.id)
      WHERE patrons.id = @PatronId;";

      MySqlParameter patron_id = new MySqlParameter();
      patron_id.ParameterName = "@PatronId";
      patron_id.Value = _id;
      cmd.Parameters.Add(patron_id);

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
      cmd.CommandText = @"DELETE FROM patrons;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
