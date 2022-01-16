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
using static Android.Views.View;

namespace CoinCatcher
{
    [Activity(Label = "IndexActivity")]
    public class IndexActivity : Activity, IOnClickListener
    {
        private BatteryBroadcast batteryBroadcast;
        private int _usrId;
        private Button btnStart, btnTop, btnRules, btnLogout, btnStartService, btnStopService;
        private TextView tv;
        //Make sure the service runs once so it won't break
        private bool _isRunning;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.index);
            // Create your application here
            _usrId = Intent.GetIntExtra("_usrId", -1);
            InitViews();
            InitObj();
        }

        //Init all the objects
        private void InitObj()
        {
            _isRunning = false;
            batteryBroadcast = new BatteryBroadcast(tv);
        }

        //Init all the views
        private void InitViews()
        {
            btnStart = FindViewById<Button>(Resource.Id.btnStart);
            btnStart.SetOnClickListener(this);
            btnTop = FindViewById<Button>(Resource.Id.btnTop);
            btnTop.SetOnClickListener(this);
            btnRules = FindViewById<Button>(Resource.Id.btnRules);
            btnRules.SetOnClickListener(this);
            btnLogout = FindViewById<Button>(Resource.Id.btnLogout);
            btnLogout.SetOnClickListener(this);
            btnStartService = FindViewById<Button>(Resource.Id.btnStartService);
            btnStartService.SetOnClickListener(this);
            btnStopService = FindViewById<Button>(Resource.Id.btnStopService);
            btnStopService.SetOnClickListener(this);
            tv = FindViewById<TextView>(Resource.Id.tv);
        }
        public void OnClick(View v)
        {
            if (btnStart == v)
            {
                //Start the GAME
                Intent game = new Intent(this, typeof(GameActivity));
                game.PutExtra("_usrId", _usrId);
                StartActivity(game);
            }
            else if (btnTop == v)
            {
                //Start new intent that shows top 10 players
                StartActivity(new Intent(this, typeof(TopPlayers)));
            }
            else if (btnRules == v)
            {
                //Start new intent that shows rules
                StartActivity(new Intent(this, typeof(RulesActivity)));
            }
            else if (btnLogout == v)
            {
                //Finish this activity which means logout
                Finish();
            }
            else if (btnStartService == v)
            {
                //Start the music Service if not already running
                if (!_isRunning)
                {
                    _isRunning = true;
                    StartService(new Intent(this, typeof(MusicService)));
                }
            }
            else if (btnStopService == v)
            {
                //Stop the music Service if running
                if (_isRunning)
                {
                    _isRunning = false;
                    StopService(new Intent(this, typeof(MusicService)));
                }
            }
        }

        protected override void OnPause()
        {
            UnregisterReceiver(batteryBroadcast);
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(batteryBroadcast, new IntentFilter(Intent.ActionBatteryChanged));
        }
    }
}