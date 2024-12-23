using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpaceWar
{
    public class BulletManager
    {
        private readonly AbsoluteLayout absoluteLayout;
        private readonly string BulletPath;
        private EnemyListClass enemyListClass;
        public BulletManager(AbsoluteLayout layout, string bulletPath,EnemyListClass e)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            BulletPath = bulletPath ?? throw new ArgumentNullException(nameof(bulletPath));
            this.enemyListClass = e;
        }

        public async void CreateNewBullet(double XPosition, double YPosition, bool isMyBullet,int MovingTime,Border myhipborder,int health)
        {
            // Create a new Bullet Image
            var newImage = new Image
            {
                Source = BulletPath,
                Aspect = Aspect.AspectFit,
                Rotation = -90,
                Opacity = 1,
            };

            AbsoluteLayout.SetLayoutBounds(newImage, new Rect(XPosition, YPosition, 27, -1));
            AbsoluteLayout.SetLayoutFlags(newImage, AbsoluteLayoutFlags.PositionProportional);

            // Check for NullReferenceException
            if (absoluteLayout == null)
            {
                throw new InvalidOperationException("absoluteLayout is not initialized.");
            }

            try
            {
                absoluteLayout.Children.Add(newImage);
                MovingBullet(newImage, isMyBullet,MovingTime,myhipborder,health);
                AnimateImage(newImage,(uint)MovingTime);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void MovingBullet(Image image, bool ismybullet,int MovingTime,Border myshipborder,int h)
        {
            double y = AbsoluteLayout.GetLayoutBounds(image).Y;
            DateTime startTime = DateTime.Now;
            while ((DateTime.Now - startTime).TotalMilliseconds < MovingTime)
            {
                await Task.Delay(35);//mermi hýzýný etkiler ters orantýlý
                if (ismybullet) y -= 0.021;
                else y += 0.021;
                // Log the current y position
                //Console.WriteLine($"Current y position: {y}");

                // Update the position
                AbsoluteLayout.SetLayoutBounds(image, new Rect(AbsoluteLayout.GetLayoutBounds(image).X, y, AbsoluteLayout.GetLayoutBounds(image).Width, AbsoluteLayout.GetLayoutBounds(image).Height));
                if (ismybullet) KonumTakipi(image, true, 1,myshipborder,h);
                else KonumTakipi(image, false, 1, myshipborder, h);
            }
        }
        private async void AnimateImage(Image image,uint MovingTime)
        {
            // Make the image visible
            image.Opacity = 1;

            // Wait for the fade-out animation to complete
            await image.FadeTo(0, MovingTime);

            // Remove the image from the layout after the animation is complete
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (absoluteLayout != null)
                {
                    absoluteLayout.Children.Remove(image);
                }
            });
        }
        private async void KonumTakipi(Image image, bool ismybullet, int damage,Border MyShipBorder,int myhealth)
        {
            //Debug.WriteLine($"{image.X}{image.Y}");
            bool hasBeenHit = false; // Bayrak deðiþkeni

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
                            /*score += 1;*/
                            absoluteLayout.Children.Remove(image);/*
                            ScoreLabel.Text = $"Score: {score}";*/
                        }
                    }
                }
            }
            else
            {
                if (IsBulletInsideEnemyBorder(image, MyShipBorder) && !hasBeenHit)
                {
                    if (myhealth > 0)
                    {
                        absoluteLayout.Children.Remove(image);
                        myhealth -= damage;
                        damage = 0;
                        //HealthLabel.Text = $"Health: {myhealth}";
                        hasBeenHit = true; // Vuruldu olarak iþaretle

                    }
                    else
                    {
                        absoluteLayout.Children.Remove(image);
                        absoluteLayout.Children.Remove(MyShipBorder);
                        //isrunnig = false;
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

