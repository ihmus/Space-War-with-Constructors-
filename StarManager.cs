using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpaceWar
{
    public class StarManager
    {
        private readonly AbsoluteLayout absoluteLayout;
        private readonly List<string> starList;
        private readonly Random random = new Random();
        private MyShipBorders myShipBorders;
        private EnemyListClass enemyListClass;
        private GeneralConstructors GeneralConstructors;
        private StarMoving StarMoving;

        public StarManager(AbsoluteLayout layout, List<string> stars, MyShipBorders myShipBorders, EnemyListClass enemyListClass,GeneralConstructors gn,StarMoving st)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            starList = stars ?? throw new ArgumentNullException(nameof(stars));
            this.myShipBorders = myShipBorders;
            this.enemyListClass = enemyListClass;
            this.GeneralConstructors = gn;
            this.StarMoving = st;
        }

        public async Task CallStarPngAsync()
        {
            Image image = null;
            var name = starList[random.Next(starList.Count)];
            var x = random.NextDouble();

            image = CreateImage(name);
            SetImageLayoutBounds(image, name, x);

            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.PositionProportional);

            if (absoluteLayout == null)
            {
                throw new InvalidOperationException("absoluteLayout is not initialized.");
            }

            try
            {
                absoluteLayout.Children.Add(image);
                await MoveImageAsync(image, name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private Image CreateImage(string name)
        {
            var image = new Image
            {
                Source = $"{name}",
                Aspect = Aspect.AspectFit,
                Opacity = 1,
            };

            if (name == "star.png")
            {
                image.Rotation = -90;
            }

            return image;
        }

        private void SetImageLayoutBounds(Image image, string name, double x)
        {
            if (name == "star.png")
            {
                AbsoluteLayout.SetLayoutBounds(image, new Rect(x, -0.05, random.Next(10, 36), -1));
            }
            else if (name == "galaksy.png")
            {
                AbsoluteLayout.SetLayoutBounds(image, new Rect(x, -0.05, random.Next(10, 80), -1));
            }
            else if (name == "meteor.png")
            {
                AbsoluteLayout.SetLayoutBounds(image, new Rect(x, -0.05, random.Next(1, 80), -1));
            }
            else if (name == "hearth.png")
            {
                AbsoluteLayout.SetLayoutBounds(image, new Rect(x, -0.05, random.Next(10, 80), -1));
            }
        }

        private async Task MoveImageAsync(Image image, string name)
        {
            int type = name switch
            {
                "star.png" => 0,
                "galaksy.png" => 1,
                "meteor.png" => 2,
                "hearth.png" => 3,
                _ => -1
            };

            if (type != -1)
            {
                MovingPng(image, type);
            }
        }

        private async void MovingPng(Image image, int id)
        {
            double y = AbsoluteLayout.GetLayoutBounds(image).Y;
            double x = AbsoluteLayout.GetLayoutBounds(image).X;
            while (true)
            {
                await Task.Delay(35);// hızını etkiler ters orantılı
                if (id == 0) y += 0.007;
                else if (id == 1) y += 0.007;
                else if (id == 2) { y += 0.007; x += 0.007; }
                else if (id == 3) y += 0.007;
                // Log the current y position
                //Console.WriteLine($"Current y position: {y}");

                // Update the position
                AbsoluteLayout.SetLayoutBounds(image, new Rect(x, y, AbsoluteLayout.GetLayoutBounds(image).Width, AbsoluteLayout.GetLayoutBounds(image).Height));
                /**************************
                 * 
                 * burası nesnelerle çarpışma kontrolünü sağlar
                 * 
                 * ************************
                 * *
                 */
                if (id == 3) KonumTakipi(image, false, 10,100);
                if (id == 2) KonumTakipi(image, false, -10,100);
            }
        }
        private async void KonumTakipi(Image image, bool ismybullet, int damage, int myhealth)
        {
            //Debug.WriteLine($"{image.X}{image.Y}");
            bool hasBeenHit = false; // Bayrak değişkeni

            if (ismybullet)
            {
                foreach (var enemyShipBorder in enemyListClass.EnemyShipBorders.ToList())
                {
                    if (IsBulletInsideEnemyBorder(image, enemyShipBorder))
                    {
                        if (absoluteLayout != null)
                        {
                            absoluteLayout.Children.Remove(enemyShipBorder);
                            enemyListClass.EnemyShipBorders.Remove(enemyShipBorder);
                            GeneralConstructors.Score+= 1;
                            absoluteLayout.Children.Remove(image);/*
                            ScoreLabel.Text = $"Score: {score}";*/
                        }
                    }
                }
            }
            else
            {
                if (IsBulletInsideEnemyBorder(image, myShipBorders.MyShipBorder) && !hasBeenHit)
                {
                    if (GeneralConstructors.Health > 0)
                    {
                        absoluteLayout.Children.Remove(image);
                        GeneralConstructors.Health += damage;
                        damage = 0;
                        //HealthLabel.Text = $"Health: {myhealth}";
                        hasBeenHit = true; // Vuruldu olarak işaretle

                    }
                    else
                    {
                        absoluteLayout.Children.Remove(image);
                        absoluteLayout.Children.Remove(myShipBorders.MyShipBorder);
                        GeneralConstructors.IsRunning = false;
                        StarMoving.Stop();
                        foreach (var enemyShipBorder in enemyListClass.EnemyShipBorders.ToList())
                        {
                            if (absoluteLayout != null)
                            {
                                absoluteLayout.Children.Remove(enemyShipBorder);
                                //generalConstructors.ScoreLabel.Text = $"Score: {generalConstructors.Score}";
                            }
                        }
                        Label OyunSonu = new Label();
                        OyunSonu.Text = "Oyun Bitti";
                        OyunSonu.TextColor = Color.FromRgb(255, 0, 0); 
                        AbsoluteLayout.SetLayoutBounds(OyunSonu, new Rect(0.5, 0.5, AbsoluteLayout.GetLayoutBounds(OyunSonu).Width, AbsoluteLayout.GetLayoutBounds(OyunSonu).Height));

                    }
                }
            }
        }

        private bool IsBulletInsideEnemyBorder(Image image, Border enemshipborder)
        {
            if (enemshipborder == null) return false;

            // Get the position and size of the EnemyShipBorder
            var enemyBounds = AbsoluteLayout.GetLayoutBounds(enemshipborder);
            var enemyRect = new Rect(
                enemyBounds.X * absoluteLayout.Width - (enemshipborder.Width / 2),
                enemyBounds.Y * absoluteLayout.Height - (enemshipborder.Height / 2),
                enemshipborder.Width,
                enemshipborder.Height
            );
            /*
            // Create a rectangle for the current position of the bullet
            var bulletBounds = AbsoluteLayout.GetLayoutBounds(bulletImage);
            var bulletRect = new Rect(
                bulletBounds.X * absoluteLayout.Width - (bulletImage.Width / 2),
                bulletBounds.Y * absoluteLayout.Height - (bulletImage.Height / 2),
                bulletImage.Width,
                bulletImage.Height
            );
            */
            // Check if the bullet's bounding box is entirely within the enemy's bounding box
            bool isWithinXBounds = image.X >= enemyRect.Left && image.X <= enemyRect.Right;
            bool isWithinYBounds = image.Y >= enemyRect.Top && image.Y <= enemyRect.Bottom;

            // Additional debugging information
            //Debug.WriteLine($"isWithinXBounds: {isWithinXBounds}");
            //Debug.WriteLine($"isWithinYBounds: {isWithinYBounds}");

            return isWithinXBounds && isWithinYBounds;
        }
    }
}
