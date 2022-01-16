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

    //Enum for knowing what side of the phone he is tilted
    enum PhoneDirection { Left, Right, Center };

    //Class represents the hero inheriting from GameObject
    class Hero : GameObject
    {
        private PhoneDirection direction;

        //C'tor
        public Hero(Context context) : base(context)
        {
            bitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.character);
            width = DisplayX / 7;
            height = DisplayY / 9;
            bitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);

            //At the beggining place at the center
            x = (DisplayX - width) / 2;
            y = DisplayY - height;

            speed = 10;
        }

        //Getter and Setter
        internal PhoneDirection Direction { get => direction; set => direction = value; }

        //Override the Move function
        public override void Move()
        {
            //Check what side the phone tilted
            switch (direction)
            {
                case PhoneDirection.Left:
                    //If tilted to the left, make sure he won't move out of boundries and move left
                    if (x - speed >= 0)
                    {
                        x -= speed;
                    }
                    break;
                case PhoneDirection.Right:
                    //If tilted to the right, make sure he won't move out of boundries and move right
                    if (x + width + speed <= DisplayX)
                    {
                        x += speed;
                    }
                    break;
                default:
                    break;
            }
        }

    }
}