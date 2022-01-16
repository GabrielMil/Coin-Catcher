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

namespace CoinCatcher.Classes
{

    //Class for the list view
    public class ScoreAdapter : BaseAdapter<ScoreItem>
    {
        Context context;
        List<ScoreItem> objects;

        //C'tor
        public ScoreAdapter(Context context, List<ScoreItem> objects)
        {
            this.context = context;
            this.objects = objects;
        }

        //Getters and Setters
        public override ScoreItem this[int position] => objects[position];

        public override int Count => objects.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        //Create the view to put in the list view
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater layoutInflater = ((TopPlayers)context).LayoutInflater;
            View view = layoutInflater.Inflate(Resource.Layout.score_item, parent, false);
            //Init the views
            TextView tvUsername = view.FindViewById<TextView>(Resource.Id.tvUsername);
            TextView tvHighscore = view.FindViewById<TextView>(Resource.Id.tvHighscore);
            TextView tvTime = view.FindViewById<TextView>(Resource.Id.tvTime);
            ScoreItem temp = objects[position];
            //Make sure the item is not null
            if (temp != null)
            {
                tvUsername.Text = temp.UserName;
                tvHighscore.Text = temp.HighScore.ToString();
                tvTime.Text = temp.Time.ToString();
            }
            return view;
        }
    }
}