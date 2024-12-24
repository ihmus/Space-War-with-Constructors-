using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpaceWar
{
    public class MyShipClass
    {
        private readonly AbsoluteLayout absoluteLayout;
        private readonly string myShipPath;
        private readonly StarMoving stars;
        private const int CreateInterval = 200, MovingTime = 1000;
        private double _startX, _startY, _xOffset, _yOffset, _currentX, _currentY;
        private int runcounter = 0;
        private const int callingtime = 1000; // Örnek deðer, ihtiyaca göre güncelleyebilirsiniz
        private PositionManager positionManager=new PositionManager();
        private EnemyListClass enemyListClass;
        private StarManager starManager;
        private CollisionManager collisionManager;
        private Label HealthLabel;
        private MyShipBorders myShipBorders;
        private GeneralConstructors generalConstructors;
        private List<String> starList = new List<String>
        {
            "star.png",
            "galaksy.png",
            "meteor.png",
            "hearth.png"
        };

        public MyShipClass(AbsoluteLayout layout, string shipPath, StarMoving stars,PositionManager position,EnemyListClass e,MyShipBorders msb,GeneralConstructors gc)
        {
            absoluteLayout = layout;
            myShipPath = shipPath;
            this.stars = stars;
            this.positionManager=position;
            this.enemyListClass = e;
            this.myShipBorders = msb;
            this.generalConstructors = gc;
        }

        public void MyShip(double x, double y)
        {
            var myShipImage = new Image
            {
                Source = myShipPath,
                Aspect = Aspect.AspectFit,
            };

            var tapGestureRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tapGestureRecognizer.Tapped += OnTapped;

            var panGestureRecognizer = new PanGestureRecognizer();
            panGestureRecognizer.PanUpdated += OnPanUpdated;

            myShipImage.GestureRecognizers.Add(tapGestureRecognizer);
            myShipImage.GestureRecognizers.Add(panGestureRecognizer);

            myShipBorders.MyShipBorder = new Border
            {
                Stroke = Colors.Green,
                StrokeThickness = 2,
                Content = myShipImage
            };

            AbsoluteLayout.SetLayoutBounds(myShipBorders.MyShipBorder, new Rect(x, y, 100, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(myShipBorders.MyShipBorder, AbsoluteLayoutFlags.PositionProportional);

            absoluteLayout.Children.Add(myShipBorders.MyShipBorder);
        }

        private async void OnTapped(object sender, EventArgs e)
        {
            if (sender is Image image && image.Parent is Border border)
            {
                double newX = _startX;
                double newY = _startY;

                double maxX = absoluteLayout.Width - border.Width;
                double maxY = absoluteLayout.Height - border.Height;

                newX = Math.Clamp(newX, -maxX, maxX);
                newY = Math.Clamp(newY, -maxY, maxY);

                border.TranslationX = newX;
                border.TranslationY = newY;

                await Task.Run(() => Console.WriteLine($"Tap moved to X: {newX}, Y: {newY}"));
            }
        }

        private async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (sender is Image image && image.Parent is Border border)
            {
                double screenWidth = absoluteLayout.Width;
                double screenHeight = absoluteLayout.Height;

                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        if (runcounter == 0)
                        {
                            generalConstructors.IsRunning = true;
                            stars.Run();
                            fired();
                            enemyfired();
                            calltheenemies();
                            runcounter += 1;
                        }
                        _xOffset = (e.TotalX - (screenWidth / 2)) / screenWidth;
                        _yOffset = (e.TotalY - (screenHeight / 2)) / screenHeight;
                        _startX = border.TranslationX;
                        _startY = border.TranslationY;
                        break;

                    case GestureStatus.Running:
                        double normalizedTotalX = e.TotalX / screenWidth;
                        double normalizedTotalY = e.TotalY / screenHeight;

                        double newX = _startX + (normalizedTotalX - _xOffset) * screenWidth;
                        double newY = _startY + (normalizedTotalY - _yOffset) * screenHeight;

                        double maxX = absoluteLayout.Width - border.Width;
                        double maxY = absoluteLayout.Height - border.Height;

                        newX = Math.Clamp(newX, -maxX, maxX);
                        newY = Math.Clamp(newY, -maxY, maxY);

                        double layoutWidth = absoluteLayout.Width;
                        double layoutHeight = absoluteLayout.Height;

                        double normalizedX = newX / layoutWidth;
                        double normalizedY = newY / layoutHeight;

                        positionManager.CurrentX = normalizedX;
                        positionManager.CurrentY = normalizedY;
                        AbsoluteLayout.SetLayoutBounds(myShipBorders.MyShipBorder, new Rect(positionManager.CurrentX, positionManager.CurrentY, AbsoluteLayout.GetLayoutBounds(myShipBorders.MyShipBorder).Width, AbsoluteLayout.GetLayoutBounds(myShipBorders.MyShipBorder).Height));

                        await Task.Run(() => Console.WriteLine($"Pan running at X: {newX}, Y: {newY}"));
                        break;

                    case GestureStatus.Completed:
                        await Task.Run(() => Console.WriteLine("Pan completed"));
                        break;
                }
            }
        }

        private async void fired()
        {
            while (generalConstructors.IsRunning)
            {
                double x = myShipBorders.MyShipBorder.X / absoluteLayout.Width;
                double y = myShipBorders.MyShipBorder.Y / absoluteLayout.Height;
                var bulletManager = BulletManagerFactory.CreateBulletManager(); // Use the bulletManager to create bullets

                bulletManager.CreateNewBullet(x + 0.01, positionManager.CurrentY, true, MovingTime, myShipBorders.MyShipBorder, 100);
                bulletManager.CreateNewBullet(x + 0.08, positionManager.CurrentY, true, MovingTime, myShipBorders.MyShipBorder, 100);
                //Debug.WriteLine($"{_currentX},{_currentX},{x},{y}");
                await Task.Delay(CreateInterval);
            }
        }

        private async void enemyfired()
        {
            double layoutWidth = absoluteLayout.Width; double layoutHeight = absoluteLayout.Height;
            if (!enemyListClass.EnemyShipBorders.Any()) // Liste boþsa {
                Console.WriteLine("Enemy ship borders list is empty.");// Döngüyü sonlandýr
            while (generalConstructors.IsRunning)
            {
                double i = 0.7;
                foreach (var enemyShipBorder in enemyListClass.EnemyShipBorders.ToList())  // ToList() kullanarak güvenli iterasyon saðlanýr
                {
                    double Y = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Y;
                    double X = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Location.X;
                    double right = X + (0.00003 * layoutWidth);
                    double left = X - (0.00003 * layoutWidth);
                    //Debug.WriteLine($"X: {X}, Right: {right}, Left: {left}"); // Konumlarý kontrol etmek için
                    if (generalConstructors.Score <= 3)
                    {
                        var bulletManager = BulletManagerFactory.CreateBulletManager(); // Use the bulletManager to create bullets
                        bulletManager.CreateNewBullet(X, Y + 0.08, false, MovingTime, myShipBorders.MyShipBorder, 100);
                    }
                    else if (generalConstructors.Score <= 8)
                    {
                        var bulletManager = BulletManagerFactory.CreateBulletManager(); // Use the bulletManager to create bullets

                        bulletManager.CreateNewBullet(right, Y + 0.08, false, MovingTime, myShipBorders.MyShipBorder, 100);
                        bulletManager.CreateNewBullet(left, Y + 0.08, false, MovingTime, myShipBorders.MyShipBorder, 100);
                    }
                    else
                    {
                        var bulletManager = BulletManagerFactory.CreateBulletManager(); // Use the bulletManager to create bullets

                        bulletManager.CreateNewBullet(X, Y + 0.08, false, MovingTime, myShipBorders.MyShipBorder, 100);
                        bulletManager.CreateNewBullet(right, Y + 0.08, false, MovingTime, myShipBorders.MyShipBorder, 100);
                        bulletManager.CreateNewBullet(left, Y + 0.08, false, MovingTime, myShipBorders.MyShipBorder, 100);

                    }

                    //Debug.WriteLine($"{Y}{X}oluþturuldu");
                    i += 0.01;
                }
                await Task.Delay(CreateInterval); // Yeni Image oluþturma aralýðý
                                                  // Example of an async task (e.g., logging)
            }
        }


        private async void calltheenemies()
        {
            while (generalConstructors.IsRunning)
            {
                var enemyManager = EnemyManagerFactory.CreateEnemyManager();
                enemyManager.Start();
                enemyManager.Start();
                enemyManager.Start();
                Callstarpng();
                Callstarpng();
                Callstarpng();

                await Task.Delay(callingtime);
            }
        }

        private void Callstarpng()
        {
            starManager = new StarManager(absoluteLayout, starList,myShipBorders,enemyListClass,generalConstructors,stars);
            starManager.CallStarPngAsync();
            // Placeholder for star creation logic
        }
    }
}
