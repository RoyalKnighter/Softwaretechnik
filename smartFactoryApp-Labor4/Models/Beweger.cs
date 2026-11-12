using System;
using System.Drawing;
using System.Collections.Generic;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        // Der Stein, den der Roboter gerade hält (null = nichts gegriffen)
        private static Beweger? dockobjekt = null;

        // NEU (UML): Winkel des gegriffenen Steins relativ zum Roboter, damit er beim Drehen mitdreht
        private static double dockWinkelOffset = 0;

        // NEU (UML): Linien, auf denen ein Stein abgelegt werden darf.
        // TODO: noch ungenutzt, im Moment darf nur auf der Delivery-Linie abgelegt werden.
        private static List<Pose>? dockLines = null;

        // NEU (UML): Linien, mit denen der Roboter kollidieren kann.
        // TODO: noch ungenutzt, KollidiertMitProduktionslinie liest die Linien aus ViewParameters.
        private static List<ProductionLine>? productionLines = null;

        // NEU (UML): Toleranz in Pixeln, um die eine Linie beim Ablegen größer gerechnet wird,
        // weil der Roboter wegen der Kollision nicht ganz an die Linie heranfahren kann
        private const int LinienToleranz = 10;

        private int x;
        private int y;
        private double angle;
        private Rectangle borders;

        // NEU (UML): Startposition bzw. "Heimat". Bei Legosteinen ist das der Platz auf ihrer
        // Produktionslinie; er pendelt mit der Linie mit, damit ein Stein dorthin zurückkann.
        protected Pose pose;

        protected const int RobotWidth = 70;   // ungedrehte Größe des Roboters, wie im XAML
        protected const int RobotHeight = 50;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public double Angle { get => angle; set => angle = value; }
        public Rectangle Borders { get => borders; set => borders = value; }

        // NEU (UML): der Stein, den der Roboter gerade hält
        public static Beweger? Dockobjekt => dockobjekt;

        // NEU (UML): aktuelle Position und Drehung zusammen
        public Pose Pose => new Pose(X, Y, Angle);

        // Linke obere Ecke als ganzzahliger Punkt
        public Point Location
        {
            get => new Point(x, y);
            set { x = value.X; y = value.Y; }
        }

        public Beweger(Point location)
        {
            Location = location;
            pose = new Pose(location.X, location.Y, 0);
        }
        public Beweger(Point location, Rectangle borders)
        {
            Location = location;
            Borders = borders;
            pose = new Pose(location.X, location.Y, 0);
        }
        public Beweger(Pose pose)
        {
            Location = pose.Location;
            Angle = pose.Angle;
            Borders = ViewParameters.Borders;
            this.pose = pose;
        }
        public Beweger(Pose pose, Rectangle borders)
        {
            Location = pose.Location;
            Angle = pose.Angle;
            Borders = borders;
            this.pose = pose;
        }

        // NEU (UML): setzt Position und Drehung auf einmal
        public void SetzePose(Pose neuePose)
        {
            X = neuePose.X;
            Y = neuePose.Y;
            Angle = neuePose.Angle;
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

        public Point Greifpunkt()
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
            Point greifPunkt = Greifpunkt();

            foreach (LegoBrick brick in bricks)
            {
                // Nur Steine, die auf einer Linie liegen, können gegriffen werden
                if (brick.Status != BrickStatus.onLine) continue;

                int x = brick.X;
                int y = brick.Y;

                if (
                    greifPunkt.X > x &&
                    greifPunkt.X < (x + 12) &&
                    greifPunkt.Y > y &&
                    greifPunkt.Y < (y + 12)
                )
                {
                    brick.Andocken();
                    dockobjekt = brick;
                    dockWinkelOffset = brick.Angle - Angle;
                    return;
                }
            }
        }

        // Lässt den Stein los, aber nur über der Delivery-Linie.
        // Gibt zurück, ob losgelassen wurde. Wohin der Stein danach fährt, entscheidet die Factory.
        public static bool Undock()
        {
            if (dockobjekt == null) return false;
            if (!AufDeliveryLinie(dockobjekt)) return false;

            // Stein gerade und mittig auf die Delivery-Linie legen
            dockobjekt.Y = ViewParameters.RectDeliveryPose.Y + ViewParameters.ProductionDeliveryHeight / 2 - 6;
            dockobjekt.Angle = ViewParameters.RectDeliveryPose.Angle;

            dockobjekt = null;
            return true;
        }

        // NEU (UML): Liegt der Mittelpunkt des 12x12-Objekts über der Delivery-Linie?
        private static bool AufDeliveryLinie(Beweger objekt)
        {
            return AufLinie(objekt, ViewParameters.RectDeliveryPose,
                            ViewParameters.ProductionDeliveryWidth, ViewParameters.ProductionDeliveryHeight);
        }

        // NEU (UML): Liegt der Mittelpunkt des 12x12-Objekts über einer der dockLines?
        private static bool AufDockLinie(Beweger objekt)
        {
            // TODO: noch nicht implementiert (dockLines wird noch nicht befüllt)
            throw new NotImplementedException();
        }

        // NEU (UML): Liegt der Mittelpunkt des 12x12-Objekts über der (ungedrehten) Linie?
        // Die Linie wird dafür um LinienToleranz in alle Richtungen größer gerechnet.
        private static bool AufLinie(Beweger objekt, Pose linie, int breite, int hoehe)
        {
            Rectangle bereich = new Rectangle(linie.X, linie.Y, breite, hoehe);
            bereich.Inflate(LinienToleranz, LinienToleranz);
            return bereich.Contains(objekt.X + 6, objekt.Y + 6);
        }

        // NEU (UML): Würde der gegriffene Stein noch in seinen Grenzen (BrickBorders) bleiben?
        public static bool DockobjektInnerhalbGrenzen()
        {
            // TODO: noch nicht implementiert
            throw new NotImplementedException();
        }

        public static void DockobjektNachziehen(Point point, double angle)
        {
            if (dockobjekt == null) return;

            dockobjekt.X = point.X - 6;
            dockobjekt.Y = point.Y - 6;
            dockobjekt.Angle = angle + dockWinkelOffset;
        }
    }
}
