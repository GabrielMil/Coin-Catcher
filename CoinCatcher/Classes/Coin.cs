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
    class Coin : Obstacle
    {
        //Create random for the type of the coin
        private static Random rand = new Random();
        //Each type has its own value
        private int _value;

        public Coin(Context context) : base(context)
        {
            int imageId = 0;
            int chance = rand.Next(0, 100);
            //There is 15 percent it will be the best coin
            if (chance < 15)
            {
                imageId = Resource.Drawable.purple_coin;
                _value = 5;
            }
            //There is 30 percent it will be the best coin
            else if (chance < 45)
            {
                imageId = Resource.Drawable.silver_coin;
                _value = 3;
            }
            //Most chances it will be this
            else
            {
                imageId = Resource.Drawable.yellow_coin;
                _value = 1;
            }
            bitmap = BitmapFactory.DecodeResource(context.Resources, imageId);
            height = width * bitmap.Height / bitmap.Width;
            bitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
        }

        public int Value { get => _value; set => _value = value; }
    }
}