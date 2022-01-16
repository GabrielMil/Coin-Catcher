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

    //Class that repressents background
    class Background : GameObject
    {

        //C'tor
        public Background(Context context) : base(context)
        {
            bitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.background);
            width = DisplayX;
            height = DisplayY;
            bitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

            x = 0;
            y = 0;
        }
    }
}