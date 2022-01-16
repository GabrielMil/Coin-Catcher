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
using System.Text.RegularExpressions;
using CoinCatcher.Classes;

namespace CoinCatcher
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        private EditText fname, lname, userName, password;
        private Button submit;
        private DatabaseHandler _db;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register);
            // Create your application here
            InitViews();
            _db = new DatabaseHandler();
        }

        //Init all the views
        private void InitViews()
        {
            fname = FindViewById<EditText>(Resource.Id.etFname);
            lname = FindViewById<EditText>(Resource.Id.etLname);
            userName = FindViewById<EditText>(Resource.Id.etUsername);
            password = FindViewById<EditText>(Resource.Id.etPassword);
            submit = FindViewById<Button>(Resource.Id.btnSubmit);
            submit.Click += Register_Submit_Click;
        }

        //Check that the password is valid
        private static bool IsPasswordValid(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
        }

        private void Register_Submit_Click(object sender, EventArgs e)
        {
            string result = "";
            User usr = new User { Fname = fname.Text, Lname = lname.Text, Username = userName.Text, Password = password.Text };
            //Validate fields
            if (usr.Fname == "" || usr.Lname == "" || usr.Username == "" || usr.Password == "")
            {
                Toast.MakeText(this, "Some fields are bad", ToastLength.Short).Show();
                return;
            }
            //Make sure paswword valid
            if (!IsPasswordValid(usr.Password))
            {
                Toast.MakeText(this, "Password doesn't match format", ToastLength.Short).Show();
                return;
            }
            //Try to add user(register)
            result = _db.AddUser(usr) ? "User was added successfully" : "Some error happened";
            Toast.MakeText(this, result, ToastLength.Short).Show();
            Finish();
        }
    }
}