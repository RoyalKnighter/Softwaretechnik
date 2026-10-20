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
                case Direction.up: dy = -s; break;
                case Direction.down: dy = s; break;
                case Direction.left: dx = -s; break;
                case Direction.right: dx = s; break;
                case Direction.upLeft: dx = -s; dy = -s; break;
                case Direction.upRight: dx = s; dy = -s; break;
                case Direction.downLeft: dx = -s; dy = s; break;
                case Direction.downRight: dx = s; dy = s; break;
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

        private bool RechteckeUeberlappen(Point[] robotPoints, int[] borderProduction)
        {
            int minRobotX = robotPoints[0].X;
            int maxRobotX = robotPoints[0].X;
            int minRobotY = robotPoints[0].Y;
            int maxRobotY = robotPoints[0].Y;

            for (int i = 1; i < 4; i++)
            {
                if (robotPoints[i].X < minRobotX) minRobotX = robotPoints[i].X;
                if (robotPoints[i].X > maxRobotX) maxRobotX = robotPoints[i].X;
                if (robotPoints[i].Y < minRobotY) minRobotY = robotPoints[i].Y;
                if (robotPoints[i].X > maxRobotY) maxRobotY = robotPoints[i].Y;
            }

            if (
                maxRobotX < borderProduction[0] ||
                borderProduction[1] < minRobotX ||
                maxRobotY < borderProduction[2] ||
                borderProduction[3] < minRobotY
                )
                return true;
            else
                return false;

        }

        protected bool KollidiertMitProduktionslinie()
        {

            return true;
        }

        protected void Drehen(Direction direction)
        {
            switch (direction)
            {
                case Direction.turnLeft:  Angle -= rotationStep; break;
                case Direction.turnRight: Angle += rotationStep; break;
            }

            // Winkel immer zwischen 0 und 360 halten
            Angle = (Angle % 360 + 360) % 360;

            // Gewünschter neuer Mittelpunkt (um ihn wird gedreht)
            double mitteX = X + RobotWidth / 2;
            double mitteY = Y + RobotHeight / 2;

            if (!Borders.IsEmpty)
            {
                // Größe der gedrehten Form
                double rad = Angle * Math.PI / 180;
                double cos = Math.Abs(Math.Cos(rad));
                double sin = Math.Abs(Math.Sin(rad));
                double breite = RobotWidth * cos + RobotHeight * sin;
                double hoehe = RobotWidth * sin + RobotHeight * cos;

                // Borders gilt für die linke obere Ecke, die Wand liegt eine Robotergröße weiter
                double wandLinks = Borders.Left;
                double wandOben = Borders.Top;
                double wandRechts = Borders.Right + RobotWidth;
                double wandUnten = Borders.Bottom + RobotHeight;

                mitteX = Math.Clamp(mitteX, wandLinks + breite / 2, wandRechts - breite / 2);
                mitteY = Math.Clamp(mitteY, wandOben + hoehe / 2, wandUnten - hoehe / 2);
            }

            X = mitteX - RobotWidth / 2;
            Y = mitteY - RobotHeight / 2;
        }
    }
}
