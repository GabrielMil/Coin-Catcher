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

namespace CoinCatcher
{

    //Class for battery BroadcastReceiver
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBatteryChanged })]
    public class BatteryBroadcast : BroadcastReceiver
    {
        //TextView to display the battery percent
        public TextView tv;

        //C'tor
        public BatteryBroadcast()
        { }

        //C'tor
        public BatteryBroadcast(TextView tv)
        {
            this.tv = tv;
        }

        //When got the percent show the percent to user and notify if needed
        public override void OnReceive(Context context, Intent intent)
        {
            //Get teh battery percent
            int battery = intent.GetIntExtra("level", 0);
            tv.Text = $"Your Battery Level: {battery}%";
            //In case battery low notify user
            if (battery <= 25)
            {
                //Build alert dialog
                AlertDialog.Builder alert = new AlertDialog.Builder(context);
                alert.SetTitle("Low battery!!!");
                alert.SetMessage($"Please charge your phone, it is {battery}%");
                alert.SetCancelable(true);
                alert.SetPositiveButton("Ok", ((senderAlert, args) => { }));

                Dialog dialog = alert.Create();
                dialog.Show();
            };
        }
    }
}