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

    //SQLite class for table Scores
    [Table("Scores")]
    public class Score
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Indexed, Column("player_id")]
        public int PlayerId { get; set; }
        [Column("high_score")]
        public int HighScore { get; set; }
        [Column("time")]
        public DateTime Time { get; set; }

        public Score() { }
    }
}