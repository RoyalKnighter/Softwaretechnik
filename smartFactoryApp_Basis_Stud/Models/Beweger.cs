using System;
using System.Drawing;
using static smartFactoryApp.Models.ViewParameters;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        private Point location; // beim Roboter die linke obere Ecke, beim Legostein der Mittelpunkt
        Rectangle borders;
        protected Beweger dockobjekt; // gedocktes Objekt (z.B. Legostein), null = Roboter ist frei
        int step = Parameters.step;

        public Point Location { get => location; set => location = value; }
        public Rectangle Borders { get => borders; set => borders = value; }
        protected int Step { set => step = value; }

        // Die Produktionslinie als Hindernis für den Roboter
        // (Position und Größe wie das Rectangle in MainWindow.axaml)
        private static readonly Rectangle productionLine = new Rectangle(
            ViewParameters.ProductionLinePosition,
            new Size(ViewParameters.ProductionLineWidth, ViewParameters.ProductionLineHeight));

        public Beweger(Point location, Rectangle borders)
        {
            this.location = location;
            this.borders = borders;
        }

        public virtual void Bewegen(Parameters.Direction dir)
        {
            Point newLoc = location;
            switch (dir)
            {
                case Parameters.Direction.up:    newLoc.Y -= step; break;
                case Parameters.Direction.down:  newLoc.Y += step; break;
                case Parameters.Direction.left:  newLoc.X -= step; break;
                case Parameters.Direction.right: newLoc.X += step; break;
                default: return;
            }

            // Nur bewegen, wenn die newLoc Position innerhalb der Grenzen liegt,
            // der Roboter dabei die Produktionslinie nicht berührt
            // und ein gedocktes Objekt ebenfalls innerhalb seiner Grenzen bleibt
          
            
                location = newLoc;
     
                
        }

        private static bool InnerhalbGrenzen(Point p, Rectangle b)
        {
           
           return true;
        }

        // Greifpunkt des Roboters: Mitte seiner Oberkante (p = linke obere Ecke des Roboters)
        private static Point Greifpunkt(Point p)
        {
            
            return (new Point(0,0));
        }

        // Prüft, ob der Roboter an Position newLoc die Produktionslinie überlappt
        private bool KollidiertMitProduktionslinie(Point newLoc)
        {
           
           return false;
        }

        // Kontakt besteht, wenn das Objekt (Location = Mittelpunkt) höchstens 10 Pixel vom Greifpunkt entfernt ist.
        // Ist der Roboter noch frei, wird ein noch nicht gedockter Legostein automatisch angedockt.
        public bool Kontakt(Beweger objekt)
        {
           bool prox = false;
            return prox;
        }

        // Gibt das gedockte Objekt wieder frei; es bleibt an seiner aktuellen Position liegen
        public bool Undock()
        {
           
            return true;
        }
    }
}
