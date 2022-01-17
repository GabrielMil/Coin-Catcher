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

    //Class represents the obstacle inheriting from GameObject
    class Obstacle : GameObject
    {
        //Create random for the places to spawn
        private static Random rand = new Random();

        //C'tor
        public Obstacle(Context context) : base(context)
        {
            width = DisplayX / 7;
            //Put random x
            x = rand.Next(0, DisplayX-width);
            y = 10;
            speed = 6;
        }

        //Override the Move function
        public override void Move()
        {
            y += speed;
        }
    }
}