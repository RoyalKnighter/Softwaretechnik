using System;
using System.Drawing;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        private Point location; // beim Roboter die linke obere Ecke, beim Legostein der Mittelpunkt
        Rectangle borders;
        protected Beweger? dockobjekt; // gedocktes Objekt (z.B. Legostein), null = Roboter ist frei
        int step = Parameters.step;

        public Point Location { get => location; set => location = value; }
        public Rectangle Borders { get => borders; set => borders = value; }
        protected int Step { set => step = value; }

        // Größe des Roboter-Rectangles in der View (MainWindow.axaml)
        private const int RobotWidth = 30;
        private const int RobotHeight = 66;

        // Die Produktionslinie als Hindernis für den Roboter
        // (Position und Größe wie das Rectangle in MainWindow.axaml)
        private static readonly Rectangle productionLine = new Rectangle(
            ViewParameters.ProductionLinePosition,
            new Size(ViewParameters.ProductionLineWidth, ViewParameters.ProductionLineHeight));

        protected static readonly Rectangle deliveryLine = new Rectangle(
            ViewParameters.DeliveryLinePosition,
            new Size(ViewParameters.DeliveryLineWidth, ViewParameters.DeliveryLineHeight));

        public Beweger(Point location, Rectangle borders)
        {
            this.location = location;
            this.borders = borders;
        }

        public virtual void Bewegen(Parameters.Direction dir)
        {
            Point neu = location;
            switch (dir)
            {
                case Parameters.Direction.up: neu.Y -= step; break;
                case Parameters.Direction.down: neu.Y += step; break;
                case Parameters.Direction.left: neu.X -= step; break;
                case Parameters.Direction.right: neu.X += step; break;
                default: return;
            }

            // Nur bewegen, wenn die neue Position innerhalb der Grenzen liegt,
            // der Roboter dabei die Produktionslinie nicht berührt
            // und ein gedocktes Objekt ebenfalls innerhalb seiner Grenzen bleibt
            if (InnerhalbGrenzen(neu, borders) &&
                !KollidiertMitProduktionslinie(neu) &&
                (dockobjekt == null || InnerhalbGrenzen(Greifpunkt(neu), dockobjekt.borders)))
            {
                location = neu;
                // Das gedockte Objekt wird am Greifpunkt gehalten
                if (dockobjekt != null)
                    dockobjekt.location = Greifpunkt(location);
            }
        }

        private static bool InnerhalbGrenzen(Point p, Rectangle b)
        {
            return p.X >= b.Left && p.X <= b.Right && p.Y >= b.Top && p.Y <= b.Bottom;
        }

        // Greifpunkt des Roboters: Mitte seiner Oberkante (p = linke obere Ecke des Roboters)
        private static Point Greifpunkt(Point p)
        {
            return new Point(p.X + RobotWidth / 2, p.Y);
        }

        // Prüft, ob der Roboter an Position neu die Produktionslinie überlappt
        private bool KollidiertMitProduktionslinie(Point neu)
        {
            // Nur der Roboter stößt an die Produktionslinie; die Legosteine fahren auf ihr
            if (this is not Robot)
                return false;

            Rectangle robot = new Rectangle(neu, new Size(RobotWidth, RobotHeight));
            return robot.IntersectsWith(productionLine);
        }

        // Kontakt besteht, wenn das Objekt (Location = Mittelpunkt) höchstens 10 Pixel vom Greifpunkt entfernt ist.
        // Ist der Roboter noch frei, wird ein noch nicht gedockter Legostein automatisch angedockt.
        public bool Kontakt(Beweger objekt)
        {
            Point g = Greifpunkt(location);
            bool ret = Math.Abs(g.X - objekt.location.X) <= 10 && Math.Abs(g.Y - objekt.location.Y) <= 10;

            if (ret && dockobjekt == null && objekt is LegoBrick aBrick && !aBrick.Gedockt)
            {
                aBrick.Gedockt = true;
                dockobjekt = objekt;
                dockobjekt.location = g;
            }
            return ret;
        }

        private bool Ausgeliefert(Rectangle line, Point p)
        {
            line.Inflate(10, 10);
            return line.Contains(p);
        }

        // Gibt das gedockte Objekt wieder frei; es bleibt an seiner aktuellen Position liegen
        public bool Undock()
        {
            if (dockobjekt == null)
                return false;
            if (!Ausgeliefert(deliveryLine, dockobjekt.Location))
                return false;
            if (dockobjekt is LegoBrick aBrick)
                aBrick.Gedockt = false;
            ((LegoBrick)dockobjekt).Ausgeliefert = true;
            dockobjekt = null;
            return true;
        }
    }
}
