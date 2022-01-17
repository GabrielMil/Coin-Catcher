using Android.App;
using Android.Content;
using Android.Graphics;
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
    class Bomb : Obstacle
    {
        public Bomb(Context context) : base(context)
        {
            bitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.bomb);
            height = width * bitmap.Height / bitmap.Width;
            bitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
        }
    }
}