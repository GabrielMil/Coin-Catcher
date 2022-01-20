using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace CoinCatcher.Classes
{
    //SQLite class for table Users
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("fname")]
        public string Fname { get; set; }
        [Column("lname")]
        public string Lname { get; set; }
        [Unique]
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }
    }
}