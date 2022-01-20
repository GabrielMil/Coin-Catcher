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

namespace CoinCatcher
{

    //Class for the music service
    [Service]
    class MusicService : Service
    {
        //Conatain the data of the music, plays and stops it
        private MediaPlayer mediaPlayer;

        //When starting Service this function plays the music
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            mediaPlayer = MediaPlayer.Create(this, Resource.Drawable.WiiMusic);
            mediaPlayer.Looping = true;
            mediaPlayer.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

        //When ends Service stop the music
        public override void OnDestroy()
        {
            mediaPlayer.Stop();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}