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
using CoinCatcher.Classes;

namespace CoinCatcher
{
    [Activity(Label = "TopPlayers")]
    public class TopPlayers : Activity
    {
        private ListView lvHighScores;
        private List<ScoreItem> scoreList;
        private ScoreAdapter scoreAA;
        private DatabaseHandler _db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.top_players);
            // Create your application here
            InitObjects();
            InitViews();
        }

        //Init objects
        private void InitObjects()
        {
            //Create connection to DB
            _db = new DatabaseHandler();
            //Get high scores and init the list
            scoreList = new List<ScoreItem>();
            List<int> ids = _db.GetHighScores();
            foreach (int id in ids)
            {
                scoreList.Add(new ScoreItem(id));
            }
            //Init Adapter
            scoreAA = new ScoreAdapter(this, scoreList);
        }

        private void InitViews()
        {
            //Init views
            lvHighScores = FindViewById<ListView>(Resource.Id.lvHighscores);
            lvHighScores.Adapter = scoreAA;
        }
    }
}