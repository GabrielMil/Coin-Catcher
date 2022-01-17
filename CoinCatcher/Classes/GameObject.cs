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

    //Class that repressents the base object of all objects in the game
    class GameObject
    {
        protected Bitmap bitmap;
        protected int x, y, width, height, speed = 0;
        protected int displayY;
        protected int displayX;

        //C'tor
        public GameObject(Context context)
        {
            var metrics = context.Resources.DisplayMetrics;
            displayX = metrics.WidthPixels;
            displayY = metrics.HeightPixels;
        }
        
        //Getters and Setters
        public Bitmap Bitmap { get => bitmap; set => bitmap = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public int Speed { get => speed; set => speed = value; }
        protected int DisplayX { get => displayX; set => displayX = value; }
        protected int DisplayY { get => displayY; set => displayY = value; }

        //Virtual function for the objects that move
        public virtual void Move()
        { }

        //Get the Rect of the shape wich help to check when there is an intersect
        public Rect GetRectShape()
        {
            return new Rect(x, y, x + width, y + height);
        }
    }
}