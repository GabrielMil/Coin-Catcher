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
    //Enum for knowing what type the obstacle is
    enum ObstacleType { Bomb, Coin }

    //Class represents the obstacle inheriting from GameObject
    class Obstacle : GameObject
    {
        //Create random for the places to spawn
        private static Random rand = new Random();
        private ObstacleType type;

        //C'tor
        public Obstacle(Context context, ObstacleType obsType) : base(context)
        {
            type = obsType;
            //Check what type should the obstacle be
            int bitmapId = (type == ObstacleType.Bomb) ? Resource.Drawable.bomb : Resource.Drawable.coin;
            bitmap = BitmapFactory.DecodeResource(context.Resources, bitmapId);
            width = DisplayX / 7;
            height = width * bitmap.Height / bitmap.Width;
            bitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
            
            //Put random x
            x = rand.Next(0, DisplayX-width);
            y = 10;
            speed = 6;
        }

        //Getter and Setter
        internal ObstacleType Type { get => type; set => type = value; }

        //Override the Move function
        public override void Move()
        {
            y += speed;
        }
    }
}