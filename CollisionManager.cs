using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceWar
{
    public class CollisionManager
    {
        private readonly AbsoluteLayout absoluteLayout;
        private readonly List<Border> enemyShipBorders;
        private readonly Border myShipBorder;
        private int score;
        private int myHealth;
        private Label scoreLabel;
        private Label healthLabel;
        private bool isRunning;

        public CollisionManager(AbsoluteLayout layout, List<Border> enemyShips, Label scoreLabel, Label healthLabel, int initialHealth)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            enemyShipBorders = enemyShips ?? throw new ArgumentNullException(nameof(enemyShips));
            this.scoreLabel = scoreLabel ?? throw new ArgumentNullException(nameof(scoreLabel));
            this.healthLabel = healthLabel ?? throw new ArgumentNullException(nameof(healthLabel));
            myHealth = initialHealth;
        }

        public async Task TrackPositionAsync(Image image, bool isMyBullet, int damage)
        {
            bool hasBeenHit = false;

            if (isMyBullet)
            {
                foreach (var enemyShipBorder in enemyShipBorders.ToList())
                {
                    if (IsBulletInsideEnemyBorder(image, enemyShipBorder))
                    {
                        RemoveEnemyShip(enemyShipBorder);
                        RemoveImage(image);
                        UpdateScore();
                    }
                }
            }
            else
            {
                if (IsBulletInsideEnemyBorder(image, myShipBorder) && !hasBeenHit)
                {
                    HandleShipHit(image, ref damage, ref hasBeenHit);
                }
            }
        }

        private bool IsBulletInsideEnemyBorder(Image image, Border enemyShipBorder)
        {
            if (enemyShipBorder == null) return false;

            var enemyBounds = AbsoluteLayout.GetLayoutBounds(enemyShipBorder);
            var enemyRect = new Rect(
                enemyBounds.X * absoluteLayout.Width - (enemyShipBorder.Width / 2),
                enemyBounds.Y * absoluteLayout.Height - (enemyShipBorder.Height / 2),
                enemyShipBorder.Width,
                enemyShipBorder.Height
            );

            bool isWithinXBounds = image.X >= enemyRect.Left && image.X <= enemyRect.Right;
            bool isWithinYBounds = image.Y >= enemyRect.Top && image.Y <= enemyRect.Bottom;

            return isWithinXBounds && isWithinYBounds;
        }

        private void RemoveEnemyShip(Border enemyShipBorder)
        {
            absoluteLayout.Children.Remove(enemyShipBorder);
            enemyShipBorders.Remove(enemyShipBorder);
        }

        private void RemoveImage(Image image)
        {
            absoluteLayout.Children.Remove(image);
        }

        private void UpdateScore()
        {
            score += 1;
            scoreLabel.Text = $"Score: {score}";
        }

        private void HandleShipHit(Image image, ref int damage, ref bool hasBeenHit)
        {
            if (myHealth > 0)
            {
                RemoveImage(image);
                myHealth -= damage;
                damage = 0;
                healthLabel.Text = $"Health: {myHealth}";
                hasBeenHit = true;
            }
            else
            {
                RemoveImage(image);
                absoluteLayout.Children.Remove(myShipBorder);
                isRunning = false;
            }
        }
    }
}
