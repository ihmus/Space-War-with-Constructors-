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

        public StarManager(AbsoluteLayout layout, List<string> stars)
        {
            absoluteLayout = layout ?? throw new ArgumentNullException(nameof(layout));
            starList = stars ?? throw new ArgumentNullException(nameof(stars));
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
                 * */
                /*
                if (id == 1) KonumTakipi(image, false, 10);
                if (id == 2) KonumTakipi(image, false, -10);
                */
            }
        }


    }
}
