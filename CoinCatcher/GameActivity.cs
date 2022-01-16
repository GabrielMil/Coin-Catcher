using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoinCatcher.Classes;
//
using Xamarin.Essentials;

namespace CoinCatcher
{
    [Activity(Label = "GameActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class GameActivity : Activity
    {
        private MediaPlayer mediaPlayer;
        private GameView gameView;
        private int _usrId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            _usrId = Intent.GetIntExtra("_usrId", -1);
            InitObj();
            SetContentView(gameView);
        }

        //Init all the objects
        private void InitObj()
        {
            gameView = new GameView(this);
            Accelerometer.ReadingChanged += gameView.Accelerometer_ReadingChanged;
            if(!Accelerometer.IsMonitoring)
            {
                Accelerometer.Start(SensorSpeed.UI);
            }
            //Background song
            mediaPlayer = MediaPlayer.Create(this, Resource.Drawable.WiiMusic);
            mediaPlayer.Looping = true;
            mediaPlayer.Start();
        }

        protected override void OnResume()
        {
            base.OnResume();
            gameView?.Resume();
            mediaPlayer?.Start();
        }

        protected override void OnPause()
        {
            gameView?.Pause();
            mediaPlayer?.Pause();
            base.OnPause();
        }

        //To finish the game build the dialog
        private void FinishGame()
        {
            gameView.IsRunning = false;
            //Build the Alert Dialog
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Finished Game");
            builder.SetMessage($"Congrats id:{_usrId}, you score is {gameView.CoinCount}");
            builder.SetCancelable(false);
            builder.SetPositiveButton("Continue", (sendAlert, args) =>
            {
                //Update the high score
                DatabaseHandler db = new DatabaseHandler();
                Score score = new Score { PlayerId = _usrId, HighScore = gameView.CoinCount, Time = DateTime.Now };
                db?.UpadateScore(score);
                //Only when pressed continue get back to index page
                base.OnBackPressed();
            });
            builder.Create().Show();
        }

        //To exit game press back(In the rules)
        public override void OnBackPressed()
        {
            FinishGame();
        }

        protected override void OnDestroy()
        {
            mediaPlayer?.Stop();
            mediaPlayer?.Release();
            if (Accelerometer.IsMonitoring)
            {
                Accelerometer.Stop();
            }
            base.OnDestroy();
        }
    }
}