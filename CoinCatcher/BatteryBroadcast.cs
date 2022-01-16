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
        TextView tv;

        //C'tor
        public BatteryBroadcast()
        { }

        //C'tor
        public BatteryBroadcast(TextView tv)
        {
            this.tv = tv;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            int battery = intent.GetIntExtra("level", 0);
            tv.Text = $"Your Battery Level: {battery}%";
        }
    }
}