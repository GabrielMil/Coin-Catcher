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

namespace CoinCatcher.Classes
{

    //Class that used for list view, to show in the top 10
    public class ScoreItem
    {
        private string _userName;
        private int _highScore;
        private DateTime _time;

        //C'tor
        public ScoreItem(int playerId)
        {
            DatabaseHandler db = new DatabaseHandler();
            User usr = db.GetUserById(playerId);
            Score score = db.GetScoreById(playerId);
            _userName = usr.Username;
            _highScore = score.HighScore;
            _time = score.Time;
        }

        //Getters and Setters
        public string UserName { get => _userName; set => _userName = value; }
        public int HighScore { get => _highScore; set => _highScore = value; }
        public DateTime Time { get => _time; set => _time = value; }
    }
}