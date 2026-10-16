using System;
using System.Drawing;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        // Position intern als Kommazahl, damit schräges Fahren nicht durch Runden verfälscht wird
        private double x;
        private double y;
        private double angle;
        private Rectangle borders;

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
        public virtual void Bewegen(Parameters.Direction direction)
        {
            double s = Parameters.step;
            double dx = 0, dy = 0;

            switch (direction)
            {
                case Parameters.Direction.up:        dy = -s;          break;
                case Parameters.Direction.down:      dy =  s;          break;
                case Parameters.Direction.left:      dx = -s;          break;
                case Parameters.Direction.right:     dx =  s;          break;
                case Parameters.Direction.upLeft:    dx = -s; dy = -s; break;
                case Parameters.Direction.upRight:   dx =  s; dy = -s; break;
                case Parameters.Direction.downLeft:  dx = -s; dy =  s; break;
                case Parameters.Direction.downRight: dx =  s; dy =  s; break;
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
    }
}
