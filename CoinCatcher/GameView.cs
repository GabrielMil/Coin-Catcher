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
using System.Threading;
using CoinCatcher.Classes;
using Xamarin.Essentials;
using Android.Media;

namespace CoinCatcher
{
    //Class for the game view, it is surface view
    class GameView : SurfaceView, ISurfaceHolderCallback
    {
        //How many obstacles are possible to be at the same time on the screen
        private static int MAX_OBSTACLES = 6;
        //Threads that responsible for running the game
        private Thread _gameThreag, _renderThread, _obstaclesGeneratorThread;
        //Part of the SurfaceView
        private ISurfaceHolder _surfaceHolder;
        //Is the game still runs and keeps going
        private bool _isRunning;
        //The sizes of the screen
        private int _displayX, _displayY;
        //Class represents the background
        private Background _background;
        //Class represents the hero
        private Hero _hero;
        //List that contains all the obstacles in the game
        private List<Obstacle> _obstacles;
        //Random generator for deciding what to spawn
        private Random _rand;
        //Coin Counter(Points)
        private int _coinCount;
        //Paint for writing the score during the game
        private Paint _scorePaint;

        //Players for the sound when player touches the obstacles
        private MediaPlayer _coinSound;
        private MediaPlayer _bombSound;

        //Getters and Setters
        public int CoinCount { get => _coinCount; set => _coinCount = value; }
        public bool IsRunning { get => _isRunning; set => _isRunning = value; }

        //C'tor
        public GameView(Context context) : base(context)
        {
            //Get screen size
            var metrics = Resources.DisplayMetrics;
            _displayX = metrics.WidthPixels;
            _displayY = metrics.HeightPixels;
            //Init for the surface view
            _surfaceHolder = Holder;
            _surfaceHolder.AddCallback(this);
            //Init wanted objects
            _rand = new Random();
            _background = new Background(context);
            _hero = new Hero(context);
            _obstacles = new List<Obstacle>();
            _coinCount = 0;
            //The paint for the score
            _scorePaint = new Paint();
            _scorePaint.TextSize = 30;
            _scorePaint.Color = Color.Black;
            //
            _coinSound = MediaPlayer.Create(Context, Resource.Drawable.coinSound);
            _bombSound = MediaPlayer.Create(Context, Resource.Drawable.bombSound);
        }

        //Function responsible for drawing on the canvas, updating UI
        public override void Draw(Canvas canvas)
        {
            //Draw the background
            canvas.DrawBitmap(_background.Bitmap, _background.X, _background.Y, null);
            //Draw the obstacles
            for (int i = 0; i < _obstacles.Count; i++)
            {
                try
                {
                    canvas.DrawBitmap(_obstacles[i].Bitmap, _obstacles[i].X, _obstacles[i].Y, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            //Draw the hero
            canvas.DrawBitmap(_hero.Bitmap, _hero.X, _hero.Y, null);
            //Draw the score
            canvas.DrawText($"YOUR SCORE: {_coinCount}", 10, 30, _scorePaint);
        }

        //When phone rotates update the moving direction of the hero
        public void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            //Phone tilted to the left
            if (data.Acceleration.X > 0.1)
            {
                _hero.Direction = PhoneDirection.Left;
            }
            //Phone tilted to the right
            else if (data.Acceleration.X < -0.1)
            {
                _hero.Direction = PhoneDirection.Right;
            }
            //Phone tilted to the center
            else
            {
                _hero.Direction = PhoneDirection.Center;
            }
        }

        //Technical function, responsible calling the Draw function for the UI
        private void Run()
        {
            //FULLY TECHNICAL, JUST RUN TEH GAME(CALL THE CHANGES TO THE UI)
            while (_isRunning)
            {
                if (_surfaceHolder.Surface.IsValid)
                {
                    var canvas = _surfaceHolder.LockCanvas();
                    Draw(canvas);
                    _surfaceHolder.UnlockCanvasAndPost(canvas);
                }
                Thread.Sleep(20);
            }
        }

        //This function updates the places of the objects, kind of the logic part
        private void Update()
        {
            while (_isRunning)
            {
                List<Obstacle> obstaclesToRemove = new List<Obstacle>();
                //Move each obstacle
                for (int i = 0; i < _obstacles.Count; i++)
                {
                    Obstacle obstacle = null;
                    try
                    {
                        obstacle = _obstacles[i];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    if (obstacle == null) continue;
                    obstacle.Move();
                    //If out of boundaries add to remove list
                    if (obstacle.Y + obstacle.Height > _displayY)
                    {
                        obstaclesToRemove.Add(obstacle);
                    }
                    //If intersected with hero also remove it
                    else if (Rect.Intersects(_hero.GetRectShape(), obstacle.GetRectShape()))
                    {
                        obstaclesToRemove.Add(obstacle);
                        //Check the type and act accordingly
                        if (obstacle is Coin coin)
                        {
                            _coinCount += coin.Value;
                            coin.Value = 0;  // Prevent duplication
                            _coinSound.Start();
                        }
                        else
                        {
                            _isRunning = false;
                            _bombSound.Start();
                        }
                    }

                }
                _hero.Move();
                //After moving all the obstacles remove the needed to be removed
                for (int i = 0; i < obstaclesToRemove.Count; i++)
                {
                    try
                    {
                        _obstacles.Remove(obstaclesToRemove.ElementAt(i));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                Thread.Sleep(20);
            }
        }

        //Function that responsible to generate obstacles
        private void GenereteObstacles()
        {
            while (_isRunning)
            {
                while (_obstacles.Count < MAX_OBSTACLES)
                {
                    //50/50 percent what type the obstacle is
                    Obstacle obstacle = null;
                    if (_rand.Next() % 2 == 0)
                    {
                        obstacle = new Coin(Context);
                    }
                    else
                    {
                        obstacle = new Bomb(Context);
                    }
                    _obstacles.Add(obstacle);
                    Thread.Sleep(650);
                }
                Thread.Sleep(1500);
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {

        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            Resume();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            Pause();
        }

        //When Activity alive
        public void Resume()
        {
            _isRunning = true;
            _gameThreag = new Thread(new ThreadStart(Update));
            _renderThread = new Thread(new ThreadStart(Run));
            _obstaclesGeneratorThread = new Thread(new ThreadStart(GenereteObstacles));

            _gameThreag.Start();
            _renderThread.Start();
            _obstaclesGeneratorThread.Start();
        }

        //When out of the activity
        public void Pause()
        {
            bool retry = true;
            while (retry)
            {
                try
                {
                    _isRunning = false;
                    _gameThreag.Join();
                    _renderThread.Join();
                    _obstaclesGeneratorThread.Join();
                    retry = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
        }

    }
}