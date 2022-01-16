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
        private MediaPlayer mediaPlayer;

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            mediaPlayer = MediaPlayer.Create(this, Resource.Drawable.WiiMusic);
            mediaPlayer.Looping = true;
            mediaPlayer.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

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