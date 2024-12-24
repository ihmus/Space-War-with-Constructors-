using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpaceWar
{
    public class EnemyManager
    {
        private readonly AbsoluteLayout absoluteLayout;
        private readonly List<string> enemyShipList;
        private readonly Random random = new Random();
        private bool isRunning;
        private double _currentX, _currentY;
        private double speed;
        private PositionManager positionManager=new PositionManager();
        private EnemyListClass enemyListClass;
        private GeneralConstructors generalConstructors;

        public EnemyManager(AbsoluteLayout layout, List<string> enemyShips, double speed,PositionManager pos, EnemyListClass enemyListClass, GeneralConstructors generalConstructors)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            enemyShipList = enemyShips ?? throw new ArgumentNullException(nameof(enemyShips));
            this.speed = speed;
            this.positionManager = pos;
            this.enemyListClass = enemyListClass;
            this.generalConstructors = generalConstructors;
        }

        public void Start()
        {
            generalConstructors.IsRunning = true;
            GenerateAndPlaceEnemyShip();
        }

        public void Stop()
        {
            generalConstructors.IsRunning = false;
        }

        private void GenerateAndPlaceEnemyShip()
        {
            // Generate a random X position between 0 and 1
            double randomX = random.NextDouble();

            // Y position at the top of the AbsoluteLayout
            double yPosition = 0;

            // Call EnemyShip1 with the random positions
            EnemyShip1(randomX, yPosition);
        }

        private async void EnemyShip1(double XPosition, double YPosition)
        {
            Image image = null;
            var name = enemyShipList[random.Next(enemyShipList.Count)];
            
            // Create the MyShip Image with Gesture Recognizers
            image = new Image
            {
                Source = $"{name}",
                Aspect = Aspect.AspectFit,
                Rotation = name == "enemyship.png" ? 180 : 0,
            };

            // Create the Border
            var enemyShipBorder = new Border
            {
                Stroke = Colors.Green,
                StrokeThickness = 2,
                Content = image,
            };

            AbsoluteLayout.SetLayoutBounds(enemyShipBorder, new Rect(XPosition, YPosition, 100, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(enemyShipBorder, AbsoluteLayoutFlags.PositionProportional);

            // Add Border to the Layout
            try
            {
                absoluteLayout.Children.Add(enemyShipBorder);
                enemyListClass.EnemyShipBorders.Add(enemyShipBorder);
                //Debug.WriteLine($"{enemyListClass.EnemyShipBorders}");
                MovingEnemies(enemyShipBorder);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void MovingEnemies(Border enemyShipBorder)
        {
            double y = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Y;
            double x = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).X;
            double myshipx = positionManager.CurrentX;
            double myshipy = positionManager.CurrentY;

            while (generalConstructors.IsRunning)
            {
                y = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Y;
                x = AbsoluteLayout.GetLayoutBounds(enemyShipBorder).X;
                myshipx = positionManager.CurrentX;
                myshipy = positionManager.CurrentY;
                await Task.Delay(16);
                y -= speed * ((y - myshipy) / Math.Sqrt(Math.Pow(y - myshipy, 2) + Math.Pow(x - myshipx, 2)));
                x -= speed * ((x - myshipx) / Math.Sqrt(Math.Pow(y - myshipy, 2) + Math.Pow(x - myshipx, 2)));

                AbsoluteLayout.SetLayoutBounds(enemyShipBorder, new Rect(x, y, AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Width, AbsoluteLayout.GetLayoutBounds(enemyShipBorder).Height));

                // Sýnýr kontrolü yap
                var enemyBounds = AbsoluteLayout.GetLayoutBounds(enemyShipBorder);
                if (y >= 1 || y < 0)
                {
                    if (absoluteLayout != null)
                    {
                        absoluteLayout.Children.Remove(enemyShipBorder);  // Remove from original list
                        enemyListClass.EnemyShipBorders.Remove(enemyShipBorder);  // Remove from original list
                    }
                    break;
                }
            }
        }
    }
}
