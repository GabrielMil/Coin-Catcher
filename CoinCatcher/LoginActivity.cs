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
using CoinCatcher.Classes;

namespace CoinCatcher
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        private EditText userName, password;
        private Button submit;
        private DatabaseHandler _db;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            // Create your application here
            //Init all
            userName = FindViewById<EditText>(Resource.Id.etUsername);
            password = FindViewById<EditText>(Resource.Id.etPassword);
            submit = FindViewById<Button>(Resource.Id.btnSubmit);
            submit.Click += Login_Submit_Click;
            _db = new DatabaseHandler();
        }

        private void Login_Submit_Click(object sender, EventArgs e)
        {
            int usrId = -1;
            string result = "";
            //Validate the values are ok
            if (userName.Text == "" || password.Text == "")
            {
                Toast.MakeText(this, "Some fields are incorrect", ToastLength.Short).Show();
                return;
            }
            //Try to login, according to respond act as needed
            usrId = _db.Login(userName.Text, password.Text);
            result = usrId != -1 ? $"Yayyy: {usrId}" : "Wrong Username or Password!";
            Toast.MakeText(this, result, ToastLength.Short).Show();
            //If logged correctly enter the index page
            if (usrId != -1)
            {
                Intent index = new Intent(this, typeof(IndexActivity));
                index.PutExtra("_usrId", usrId);
                StartActivity(index);
            }
            Finish();
        }
    }
}