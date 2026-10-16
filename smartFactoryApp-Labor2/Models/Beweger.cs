using System;
using System.Drawing;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        // Intern exakt als Kommazahl speichern, nach außen als Point
        private double posX, posY;
        private double angle;
        private Rectangle borders;

        public Point Location
        {
            get => new Point((int)Math.Round(posX), (int)Math.Round(posY));
            set { posX = value.X; posY = value.Y; }
        }
        public double Angle { get => angle; set => angle = value; }
        public Rectangle Borders { get => borders; set => borders = value; }

        // Ungedrehte Größe, wird von Robot bzw. LegoBrick gesetzt
        public double BodyWidth { get; protected set; }
        public double BodyHeight { get; protected set; }

        public Beweger(Point location, Rectangle borders)
        {
            Location = location;
            Borders = borders;
        }
        public Beweger(Point location)
        {
            Location = location;
        }
        public Beweger(Pose pose)
        {
            Location = pose.Location;
            angle = pose.Angle;
            Borders = ViewParameters.Borders;
        }

        public virtual void Bewegen(Parameters.Direction direction)
        {
            double s = Parameters.step;
            double rad = Angle * Math.PI / 180;
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
                case Parameters.Direction.forward:   dx =  Math.Cos(rad) * s; dy =  Math.Sin(rad) * s; break;
                case Parameters.Direction.backward:  dx = -Math.Cos(rad) * s; dy = -Math.Sin(rad) * s; break;
                case Parameters.Direction.turnLeft:  Drehen(-Parameters.rotationStep); return;
                case Parameters.Direction.turnRight: Drehen( Parameters.rotationStep); return;
                default: return;
            }

            Verschieben(dx, dy);
        }

        protected void Drehen(double grad)
        {
            Angle = ((Angle + grad) % 360 + 360) % 360;
            Verschieben(0, 0);   // Ecke nach Drehung wieder ins Feld schieben
        }

        private (double breite, double hoehe) GedrehteGroesse()
        {
            double rad = Angle * Math.PI / 180;
            double cos = Math.Abs(Math.Cos(rad));
            double sin = Math.Abs(Math.Sin(rad));
            return (BodyWidth * cos + BodyHeight * sin,
                    BodyWidth * sin + BodyHeight * cos);
        }

        protected void Verschieben(double dx, double dy)
        {
            double mitteX = posX + BodyWidth / 2 + dx;
            double mitteY = posY + BodyHeight / 2 + dy;

            if (!Borders.IsEmpty)
            {
                // Borders ist der Bereich für die linke obere Ecke.
                // Spielfeldrand = Borders plus ungedrehte Größe.
                double feldLinks  = Borders.Left;
                double feldOben   = Borders.Top;
                double feldRechts = Borders.Right  + BodyWidth;
                double feldUnten  = Borders.Bottom + BodyHeight;

                var (breite, hoehe) = GedrehteGroesse();

                mitteX = Math.Clamp(mitteX, feldLinks + breite / 2, feldRechts - breite / 2);
                mitteY = Math.Clamp(mitteY, feldOben  + hoehe  / 2, feldUnten  - hoehe  / 2);
            }

            posX = mitteX - BodyWidth / 2;
            posY = mitteY - BodyHeight / 2;
        }
    }
}