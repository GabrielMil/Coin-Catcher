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
        private static int MAX_OBSTACLES = 5;
        private Thread gameThreag, renderThread, obstaclesGeneratorThread;
        private ISurfaceHolder surfaceHolder;
        private bool isRunning;
        private int displayX, displayY;
        private Background background;
        private Hero hero;
        private List<Obstacle> obstacles;
        private Random rand;
        private int coinCount;
        private Paint scorePaint;

        private MediaPlayer coinSound;
        private MediaPlayer bombSound;

        private Mutex mutex;

        //Getters and Setters
        public int CoinCount { get => coinCount; set => coinCount = value; }
        public bool IsRunning { get => isRunning; set => isRunning = value; }

        //C'tor
        public GameView(Context context) : base(context)
        {
            //Get screen size
            var metrics = Resources.DisplayMetrics;
            displayX = metrics.WidthPixels;
            displayY = metrics.HeightPixels;
            //Init for the surface view
            surfaceHolder = Holder;
            surfaceHolder.AddCallback(this);
            //Init wanted objects
            rand = new Random();
            background = new Background(context);
            hero = new Hero(context);
            obstacles = new List<Obstacle>();
            coinCount = 0;
            //The paint for the score
            scorePaint = new Paint();
            scorePaint.TextSize = 30;
            scorePaint.Color = Color.Black;
            //
            coinSound = MediaPlayer.Create(Context, Resource.Drawable.coinSound);
            bombSound = MediaPlayer.Create(Context, Resource.Drawable.bombSound);
            //

            mutex = new Mutex();
        }

        //Function responsible for drawing on the canvas, updating UI
        public override void Draw(Canvas canvas)
        {
            //Draw the background
            canvas.DrawBitmap(background.Bitmap, background.X, background.Y, null);
            //Draw the obstacles
            mutex.WaitOne();
            for (int i = 0; i < obstacles.Count; i++)
            {
                canvas.DrawBitmap(obstacles[i].Bitmap, obstacles[i].X, obstacles[i].Y, null);
            }
            mutex.ReleaseMutex();
            //Draw the hero
            canvas.DrawBitmap(hero.Bitmap, hero.X, hero.Y, null);
            //Draw the score
            canvas.DrawText($"YOUR SCORE: {coinCount}", 10, 30, scorePaint);
        }

        public void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            //Phone titlted to the left
            if (data.Acceleration.X > 0.1)
            {
                hero.Direction = PhoneDirection.Left;
            }
            //Phone titlted to the right
            else if (data.Acceleration.X < -0.1)
            {
                hero.Direction = PhoneDirection.Right;
            }
            //Phone titlted to the center
            else
            {
                hero.Direction = PhoneDirection.Center;
            }
        }

        //Technical function, responsbile calling the Draw function for the UI
        private void Run()
        {
            //FULLY TECHNICAL, JUST RUN TEH GAME(CALL THE CHANGES TO THE UI)
            Canvas canvas = null;
            while (isRunning)
            {
                if (surfaceHolder.Surface.IsValid)
                {
                    canvas = surfaceHolder.LockCanvas();
                    Draw(canvas);
                    surfaceHolder.UnlockCanvasAndPost(canvas);
                }
                Thread.Sleep(24);
            }
        }

        //This function updates the places of the objects, kind of the logic part
        private void Update()
        {
            while (isRunning)
            {
                List<Obstacle> obstaclesToRemove = new List<Obstacle>();
                //Move each obstacle
                for (int i = 0; i < obstacles.Count; i++)
                {
                    Obstacle obstacle = obstacles[i];
                    obstacle.Move();
                    //If out of boundries add to remove list
                    if (obstacle.Y + obstacle.Height > displayY)
                    {
                        obstaclesToRemove.Add(obstacle);
                    }
                    //If intesected with hero also remove it
                    else if (Rect.Intersects(hero.GetRectShape(), obstacle.GetRectShape()))
                    {
                        obstaclesToRemove.Add(obstacle);
                        //Check the type and act accordingly
                        if (obstacle.Type == ObstacleType.Coin)
                        {
                            coinCount++;
                            coinSound.Start();
                        }
                        else
                        {
                            isRunning = false;
                            bombSound.Start();
                        }
                    }
                }
                hero.Move();
                //After moving all the obstacles remove the needed to be remoev
                for (int i = 0; i < obstaclesToRemove.Count; i++)
                {
                    mutex.WaitOne();
                    obstacles.Remove(obstaclesToRemove.ElementAt(i));
                    mutex.ReleaseMutex();
                }
                Thread.Sleep(20);
            }
        }

        //Function that responsible to generate obstacles
        private void GenereteObstacles()
        {
            while (isRunning)
            {
                while (obstacles.Count < MAX_OBSTACLES)
                {
                    mutex.WaitOne();
                    //Spawn the  obstacle and choose teh type randomly
                    ObstacleType type = (rand.Next() % 2 == 0) ? ObstacleType.Bomb : ObstacleType.Coin;
                    obstacles.Add(new Obstacle(Context, type));
                    mutex.ReleaseMutex();
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
            isRunning = true;
            gameThreag = new Thread(new ThreadStart(Update));
            renderThread = new Thread(new ThreadStart(Run));
            obstaclesGeneratorThread = new Thread(new ThreadStart(GenereteObstacles));

            gameThreag.Start();
            renderThread.Start();
            obstaclesGeneratorThread.Start();
        }

        //When out of the activity
        public void Pause()
        {
            bool retry = true;
            while (retry)
            {
                try
                {
                    isRunning = false;
                    gameThreag.Join();
                    renderThread.Join();
                    obstaclesGeneratorThread.Join();
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