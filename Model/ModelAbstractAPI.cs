﻿using System;
using System.Diagnostics;
using Logic;
using Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

using System.Windows.Media.Animation;


namespace Model
{
    public abstract class ModelAbstractAPI
    {
        //public abstract void StartSimulation(int nrOfBalls);
        public abstract void CreateEllipses(int nrOfBalls);
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ellipseCollection { get; }

        public static ModelAbstractAPI CreateModelAPI()
        {
            return new ModelAPI();
        }
    }

    public class ModelAPI : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicAPI;
        private readonly DataAbstractAPI dataAPI;
        public override List<Ellipse> ellipseCollection { get; }
        public override Canvas Canvas { get; set; }
        private readonly Random random;

        private List<Storyboard> ballAnimations;

        public ModelAPI()
        {
            dataAPI = DataAbstractAPI.CreateAPI();
            logicAPI = LogicAbstractAPI.CreateLogicAPI();
            Canvas = new Canvas();
            ellipseCollection = new List<Ellipse>();
            Canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            Canvas.VerticalAlignment = VerticalAlignment.Stretch;
            Canvas.Width = 800;
            Canvas.Height = 600;
            random = new Random();

            ballAnimations = new List<Storyboard>();
        }

        public void StartBallAnimation()
        {
            foreach (var ellipse in ellipseCollection)
            {
                double speedX = GetRandomSpeed(); 
                double speedY = GetRandomSpeed(); 

                CompositionTarget.Rendering += (sender, e) =>
                {
                    double newX = Canvas.GetLeft(ellipse) + speedX;
                    double newY = Canvas.GetTop(ellipse) + speedY;

                    // Sprawdź, czy kulka dotknęła krawędzi ramki i jeśli tak, odwróć jej kierunek
                    if (newX >= Canvas.ActualWidth - ellipse.Width || newX <= 0)
                    {
                        speedX *= -1; // Odwróć kierunek w osi X
                    }

                    if (newY >= Canvas.ActualHeight - ellipse.Height || newY <= 0)
                    {
                        speedY *= -1; // Odwróć kierunek w osi Y
                    }

                    // Aktualizuj pozycję kulki
                    Canvas.SetLeft(ellipse, newX);
                    Canvas.SetTop(ellipse, newY);
                };
            }
        }

        private double GetRandomSpeed()
        {
            Random random = new Random();
            return random.NextDouble() * 5 + 1; // Losowa prędkość z zakresu 1-6
        }

        public void StopBallAnimation()
        {
            foreach (var storyboard in ballAnimations)
            {
                storyboard.Pause();
            }
        }


        public override void CreateEllipses(int numberOfBalls)
        {
            logicAPI.Start(numberOfBalls);

            for (int i = 0; i < numberOfBalls; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(150, 256), (byte)random.Next(0, 1), (byte)random.Next(150, 256)));
                Ellipse ellipse = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Fill = brush
                };

                double x = random.Next(30, (int)Canvas.Width - 30);
                double y = random.Next(30, (int)Canvas.Height - 30);

                // Sprawdzamy, czy nowa elipsa nie nakłada się na istniejące elipsy
                bool isOverlapping = CheckForOverlap(x, y);
                while (isOverlapping)
                {
                    x = random.Next(0, (int)Canvas.Width - 10);
                    y = random.Next(0, (int)Canvas.Height - 10);
                    isOverlapping = CheckForOverlap(x, y);
                }

                // Ustawiamy pozycję nowej elipsy
                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }
        }

        private bool CheckForOverlap(double x, double y)
        {
            foreach (var existingEllipse in ellipseCollection)
            {
                double existingX = Canvas.GetLeft(existingEllipse);
                double existingY = Canvas.GetTop(existingEllipse);

                // Sprawdzamy czy nowa elipsa nakłada się na istniejącą elipsę
                if (Math.Abs(existingX - x) < 20 && Math.Abs(existingY - y) < 20)
                {
                    return true;
                }
            }
            return false;
        }

    }
}