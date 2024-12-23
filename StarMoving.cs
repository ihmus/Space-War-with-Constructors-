using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpaceWar
{
    public class StarMoving
    {
        private static readonly Random random = new Random();
        private static readonly string[] starImages = { "star.png", "star1.png", "star2.png", "star3.png" };
        private AbsoluteLayout _layout;
        private int _numberOfStars;
        private bool _isRunning=false;

        public StarMoving(AbsoluteLayout layout, int numberOfStars)
        {
            _layout = layout;
            _numberOfStars = numberOfStars;
        }

        public void Run()
        {
            _isRunning = true;
            AddStars(_isRunning);
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private void AddStars(bool isRunning)
        {
            for (int i = 0; i < _numberOfStars; i++)
            {
                string starImage = starImages[random.Next(starImages.Length)];
                Image star = new Image
                {
                    Source = starImage,
                    WidthRequest = starImage == "star.png" ? random.Next(1, 21) : 15,
                    HeightRequest = -1
                };

                double xPos = random.NextDouble();
                double yPos = random.NextDouble();
                AbsoluteLayout.SetLayoutBounds(star, new Rect(xPos, yPos, -1, -1));
                AbsoluteLayout.SetLayoutFlags(star, AbsoluteLayoutFlags.PositionProportional);

                _layout.Children.Add(star);
                StartStarMoving(star, _layout, isRunning);

                if (starImage == "star.png")
                {
                    StartStarAnimation(star, isRunning, (uint)random.Next(150, 200));
                }
            }
        }

        private async void StartStarMoving(Image star, AbsoluteLayout layout, bool isRunning)
        {
            double screenHeight = layout.Height;
            double xPos = AbsoluteLayout.GetLayoutBounds(star).X;
            double yPos = AbsoluteLayout.GetLayoutBounds(star).Y;

            while (isRunning)
            {
                await Task.Delay(33);
                yPos += 0.005; // Increment Y to move the star downwards
                if (yPos > 1) // Reset position if the star reaches the bottom
                {
                    yPos = -AbsoluteLayout.GetLayoutBounds(star).Height / layout.Height - 0.1;
                    xPos = random.NextDouble();
                }
                AbsoluteLayout.SetLayoutBounds(star, new Rect(xPos, yPos, AbsoluteLayout.GetLayoutBounds(star).Width, AbsoluteLayout.GetLayoutBounds(star).Height));
            }
            Debug.WriteLine($"{isRunning}");
        }

        private async void StartStarAnimation(Image star, bool isRunning, uint shineTime)
        {
            while (isRunning)
            {
                await star.FadeTo(0.5, shineTime);
                await star.FadeTo(1, shineTime);
            }
            Debug.WriteLine($"{isRunning}");
        }
    }
}
