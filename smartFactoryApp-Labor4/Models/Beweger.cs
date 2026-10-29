using System;
using System.Drawing;
using System.Collections.Generic;
using static smartFactoryApp.Models.Parameters;
using Avalonia.Controls;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        public static Beweger dockObjekt = null;
        // Position intern als Kommazahl, damit schräges Fahren nicht durch Runden verfälscht wird
        private int x;
        private int y;
        private double angle;
        private Rectangle borders;
        protected const int RobotWidth = 70;   // ungedrehte Größe des Roboters, wie im XAML
        protected const int RobotHeight = 50;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public double Angle { get => angle; set => angle = value; }
        public Rectangle Borders { get => borders; set => borders = value; }

        // Linke obere Ecke als ganzzahliger Punkt
        public Point Location
        {
            get => new Point(x, y);
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
            int s = step;
            int dx = 0, dy = 0;

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

        protected void Drehen(Direction direction)
        {
            double angle = Angle;
            switch (direction)
            {
                case Direction.turnLeft: angle -= rotationStep; break;
                case Direction.turnRight: angle += rotationStep; break;
            }

            // Winkel immer zwischen 0 und 360 halten
            angle = (angle % 360 + 360) % 360;

            // Gewünschter neuer Mittelpunkt (um ihn wird gedreht)
            double mitteX = X + RobotWidth / 2;
            double mitteY = Y + RobotHeight / 2;

            if (!Borders.IsEmpty)
            {
                // Größe der gedrehten Form
                double rad = angle * Math.PI / 180;
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

            int x = (int)Math.Round(mitteX - RobotWidth / 2);
            int y = (int)Math.Round(mitteY - RobotHeight / 2);

            if (!KollidiertMitProduktionslinie(x, y))
            {
                Angle = angle;
                X = x;
                Y = y;
            }
        }

        // Bekommt die 4 Ecken zweier Rechtecke, jeweils der Reihe nach rundherum.
        private bool RechteckeUeberlappen(PointF[] a, PointF[] b)
        {
            // Erst in der Lage von b prüfen, dann in der Lage von a
            return !LueckeInLageVon(b) && !LueckeInLageVon(a);

            // Dreht beide Rechtecke so, dass "rechteck" gerade liegt (0°),
            // und prüft dann wie bei ungedrehten Rechtecken, ob es eine Lücke gibt.
            bool LueckeInLageVon(PointF[] rechteck)
            {
                // Winkel des Rechtecks aus seiner ersten Kante
                double winkel = Math.Atan2(rechteck[1].Y - rechteck[0].Y,
                                           rechteck[1].X - rechteck[0].X);
                double cos = Math.Cos(-winkel);
                double sin = Math.Sin(-winkel);

                var (minAX, maxAX, minAY, maxAY) = Grenzen(a, cos, sin);
                var (minBX, maxBX, minBY, maxBY) = Grenzen(b, cos, sin);

                return maxAX < minBX || maxBX < minAX ||
                       maxAY < minBY || maxBY < minAY;
            }

            // Dreht alle Ecken und liefert links, rechts, oben, unten
            static (double minX, double maxX, double minY, double maxY) Grenzen(
                PointF[] ecken, double cos, double sin)
            {
                double minX = double.MaxValue, maxX = double.MinValue;
                double minY = double.MaxValue, maxY = double.MinValue;

                foreach (PointF e in ecken)
                {
                    double x = e.X * cos - e.Y * sin;
                    double y = e.X * sin + e.Y * cos;

                    minX = Math.Min(minX, x); maxX = Math.Max(maxX, x);
                    minY = Math.Min(minY, y); maxY = Math.Max(maxY, y);
                }
                return (minX, maxX, minY, maxY);
            }
        }

        protected bool KollidiertMitProduktionslinie(int x, int y)
        {
            PointF[] roboter = Ecken(x + RobotWidth / 2.0, y + RobotHeight / 2.0,
                                     RobotWidth, RobotHeight, Angle);

            double w = ViewParameters.ProductionLineWidth;
            double h = ViewParameters.ProductionLineHeight;

            Pose[] linien =
            {
                ViewParameters.RectRedPose,   ViewParameters.RectBluePose,
                ViewParameters.RectGreenPose, ViewParameters.RectYellowPose,
                ViewParameters.RectRecyclePose
            };

            PointF[] ecken = Ecken(
                ViewParameters.RectDeliveryPose.X + ViewParameters.ProductionDeliveryWidth / 2,
                ViewParameters.RectDeliveryPose.Y + ViewParameters.ProductionDeliveryHeight / 2,
                ViewParameters.ProductionDeliveryWidth,
                ViewParameters.ProductionDeliveryHeight,
                ViewParameters.RectDeliveryPose.Angle
                );
            if (RechteckeUeberlappen(roboter, ecken))
                return true;

            foreach (Pose linie in linien)
            {
                ecken = Ecken(linie.X + w / 2, linie.Y + h / 2, w, h, linie.Angle);
                if (RechteckeUeberlappen(roboter, ecken))
                    return true;
            }

            return false;

            // Die 4 Ecken eines um seine Mitte gedrehten Rechtecks, der Reihe nach rundherum
            static PointF[] Ecken(double mitteX, double mitteY, double w, double h, double winkel)
            {
                double rad = winkel * Math.PI / 180;
                double cos = Math.Cos(rad), sin = Math.Sin(rad);

                double[] lx = { -w / 2, w / 2, w / 2, -w / 2 };
                double[] ly = { -h / 2, -h / 2, h / 2, h / 2 };

                var ecken = new PointF[4];
                for (int i = 0; i < 4; i++)
                {
                    ecken[i] = new PointF((float)(mitteX + lx[i] * cos - ly[i] * sin),
                                          (float)(mitteY + lx[i] * sin + ly[i] * cos));
                }
                return ecken;
            }
        }

        public Point GreifPunkt()
        {
            double rad = Angle * Math.PI / 180;
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);
            
            double mitteX = X + RobotWidth / 2;
            double mitteY = Y + RobotHeight / 2;

            return new Point((int) Math.Round(mitteX + sin * RobotHeight / 2),
                             (int) Math.Round(mitteY - cos * RobotHeight / 2));
        }

        public void Kontakt(List<LegoBrick> bricks)
        {
            Point greifPunkt = GreifPunkt();

            foreach (LegoBrick brick in bricks)
            {
                // Steine auf der Delivery-Linie dürfen nicht wieder gegriffen werden
                if (brick.Ausgeliefert) continue;

                int x = brick.X;
                int y = brick.Y;

                if (
                    greifPunkt.X > x &&
                    greifPunkt.X < (x + 12) &&
                    greifPunkt.Y > y &&
                    greifPunkt.Y < (y + 12)
                )
                {
                    brick.Gedockt = true;
                    dockObjekt = brick;
                    return;
                }
            }
        }

        // Lässt den Stein los, aber nur über der Delivery-Linie.
        // Gibt zurück, ob losgelassen wurde.
        public static bool Undock()
        {
            if (dockObjekt is not LegoBrick brick) return false;

            // Delivery-Linie mit 10 Pixel Toleranz, weil der Roboter wegen der
            // Kollision nicht ganz an die Linie heranfahren kann
            Rectangle delivery = new Rectangle(
                ViewParameters.RectDeliveryPose.X, ViewParameters.RectDeliveryPose.Y,
                ViewParameters.ProductionDeliveryWidth, ViewParameters.ProductionDeliveryHeight);
            delivery.Inflate(10, 10);

            // Mittelpunkt des 12x12-Steins muss über der Linie liegen
            if (!delivery.Contains(brick.X + 6, brick.Y + 6)) return false;

            brick.Gedockt = false;
            brick.Ausgeliefert = true;

            // Stein gerade und mittig auf die Delivery-Linie legen
            brick.Y = ViewParameters.RectDeliveryPose.Y + ViewParameters.ProductionDeliveryHeight / 2 - 6;
            brick.Angle = ViewParameters.RectDeliveryPose.Angle;

            dockObjekt = null;
            return true;
        }

        public static void DockobjektNachziehen(Point point, double angle)
        {
            if (dockObjekt == null) return;

            dockObjekt.X = point.X - 6;
            dockObjekt.Y = point.Y - 6;
            dockObjekt.Angle = angle;
        }
    }
}
