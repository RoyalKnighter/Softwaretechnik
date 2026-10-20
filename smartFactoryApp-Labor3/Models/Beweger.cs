using System;
using System.Drawing;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        // Position intern als Kommazahl, damit schräges Fahren nicht durch Runden verfälscht wird
        private double x;
        private double y;
        private double angle;
        private Rectangle borders;
        protected const double RobotWidth = 70;   // ungedrehte Größe des Roboters, wie im XAML
        protected const double RobotHeight = 50;

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Angle { get => angle; set => angle = value; }
        public Rectangle Borders { get => borders; set => borders = value; }

        // Linke obere Ecke als ganzzahliger Punkt
        public Point Location
        {
            get => new Point((int)Math.Round(x), (int)Math.Round(y));
            set { x = value.X; y = value.Y; }
        }

        public Beweger(Point location)
        {
            Location = location;
        }
        public Beweger(Point location, Rectangle borders)
        {
            Location = location;
            Borders = borders;
        }
        public Beweger(Pose pose)
        {
            Location = pose.Location;
            Angle = pose.Angle;
            Borders = ViewParameters.Borders;
        }
        public Beweger(Pose pose, Rectangle borders)
        {
            Location = pose.Location;
            Angle = pose.Angle;
            Borders = borders;
        }

        // Einfache Bewegung um einen Schritt. Die linke obere Ecke muss in Borders bleiben,
        // sonst wird der Schritt in dieser Achse nicht ausgeführt.
        public virtual void Bewegen(Direction direction)
        {
            double s = step;
            double dx = 0, dy = 0;

            switch (direction)
            {
                case Direction.up:        dy = -s;          break;
                case Direction.down:      dy =  s;          break;
                case Direction.left:      dx = -s;          break;
                case Direction.right:     dx =  s;          break;
                case Direction.upLeft:    dx = -s; dy = -s; break;
                case Direction.upRight:   dx =  s; dy = -s; break;
                case Direction.downLeft:  dx = -s; dy =  s; break;
                case Direction.downRight: dx =  s; dy =  s; break;
                default: return;
            }

            if (Borders.IsEmpty)
            {
                X += dx;
                Y += dy;
                return;
            }

            if (X + dx >= Borders.Left && X + dx <= Borders.Right)
                X += dx;
            if (Y + dy >= Borders.Top && Y + dy <= Borders.Bottom)
                Y += dy;
        }

        private bool RechteckeUeberlappen(Point [] a, Point [] b)
        {
            int minAX = a[0].X;
            int maxAX = a[0].X;
            int minAY = a[0].Y;
            int maxAY = a[0].Y;

            int minBX = a[0].X;
            int maxBX = a[0].X;
            int minBY = a[0].Y;
            int maxBY = a[0].Y;

            for (int i = 1; i < 4; i++)
            {
                if (a[i].X < minAX) minAX = a[i].X;
                if (a[i].X > maxAX) maxAX = a[i].X;
                if (a[i].Y < minAY) minAY = a[i].Y;
                if (a[i].X > maxAY) maxAY = a[i].Y;

                if (b[i].X < minBX) minBX = b[i].X;
                if (b[i].X > maxBX) maxBX = b[i].X;
                if (b[i].Y < minBY) minBY = b[i].Y;
                if (b[i].X > maxBY) maxBY = b[i].Y;
            }

            if (
                maxAX < minBX ||
                maxBX < minAX ||
                maxAY < minBY ||
                maxBY < minAY
                )
                return true;
            else
                return false;
            
        }

        protected bool KollidiertMitProduktionslinie()
        {
            










            return true;
        }
    }
}
