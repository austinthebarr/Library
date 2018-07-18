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

      com.ExecuteNonQuery();

      conn.Close();
      if (Conn != null)
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
      cmd.Parameter.Add(title);

      MySqlParameter genre = new MySqlParameter();
      genre.ParameterName = "@BookGenre";
      genre.Value = _genre;
      cmd.Parameter.Add(genre);

      MySqlParameter inventory = new MySqlParameter();
      inventory.ParameterName = "@BookInventory";
      inventory.Value = _inventory;
      cmd.Parameter.Add(inventory);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}
