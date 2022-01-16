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
using SQLite;

namespace CoinCatcher.Classes
{
    //Class that helps me handel and use the DB
    public class DatabaseHandler
    {
        private readonly string _path = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myCoolDB");
        private SQLiteConnection _db;

        //Constructor, create connection adn create tables if not exists
        public DatabaseHandler()
        {
            _db = new SQLiteConnection(_path);
            _db.CreateTable<User>();
            _db.CreateTable<Score>();
        }

        //Get user and add him to table
        public bool AddUser(User usr)
        {
            bool isValid = false;
            //Add new user to DB
            try
            {
                _db.Insert(usr);
                isValid = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return isValid;
        }


        //Get Score and insert first time or update it
        public bool UpadateScore(Score score)
        {
            bool isValid = false;
            try
            {
                //First check if score exist, if not, insert
                Score currHighScore = GetScoreById(score.PlayerId);
                if (null == currHighScore)
                {
                    _db.Insert(score);
                }
                else
                {
                    //Make sure you update only if you broke your score
                    if (currHighScore.HighScore < score.HighScore)
                    {
                        score.Id = currHighScore.Id;
                        _db.Update(score);
                    }
                }
                isValid = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return isValid;
        }

        //Try to login, if logged successfully get user id, else return -1
        public int Login(string username, string password)
        {
            int id = -1;
            try
            {
                //Select with where statement
                id = _db.Table<User>().Where(usr => usr.Username == username && usr.Password == password).ToList()[0].Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return id;
        }

        //Get top 10 player id's
        public List<int> GetHighScores()
        {
            List<int> ids = new List<int>();
            try
            {
                //Select and order by
                var scores = _db.Table<Score>().OrderByDescending(hs => hs.HighScore).ToList();
                //Foreach score add player's id
                foreach (Score score in scores)
                {
                    ids.Add(score.PlayerId);
                }
            }
            catch { }
            return ids;
        }

        //Return user by its Id
        public User GetUserById(int id)
        {
            User usr = null;
            try
            {
                //Select query with where
                usr = _db.Table<User>().Where(u => u.Id == id).ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return usr;
        }

        //Return score by player's Id
        public Score GetScoreById(int id)
        {
            Score score = null;
            try
            {
                //Select query with where
                score = _db.Table<Score>().Where(u => u.PlayerId == id).ToList()[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return score;
        }
    }
}