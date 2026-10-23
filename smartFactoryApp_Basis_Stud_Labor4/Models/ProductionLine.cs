using System.Collections.Generic;
using System.Drawing;

namespace smartFactoryApp.Models
{
    internal class ProductionLine
    {
        private Point location; // linke obere Ecke

        // Die Legosteine, die auf dieser Produktionslinie liegen
        private List<LegoBrick> bricks = new List<LegoBrick>();

        public Point Location { get => location; }
        public Parameters.LegoColor Color { get; }
        public IReadOnlyList<LegoBrick> Bricks => bricks;

        public ProductionLine(Point location, Parameters.LegoColor color)
        {
            this.location = location;
            Color = color;
            CreateBricks();
        }

        // Erzeugt die Legosteine in der Farbe der Linie, gleichmäßig entlang der Linie verteilt
        // und nahe an ihrer Unterkante, damit der Roboter sie von unten greifen kann
        private void CreateBricks()
        {
            int count = Parameters.bricksPerLine;
            int spacing = ViewParameters.ProductionLineWidth / count;

            for (int i = 0; i < count; i++)
            {
                var brickLocation = new Point(location.X + i * spacing + spacing / 2 - 10,
                                              location.Y + ViewParameters.ProductionLineHeight - 5);
                bricks.Add(new LegoBrick(brickLocation, Color));
            }
        }

        // Zähler für die Timer-Takte: die ersten 10 Takte nach rechts, die nächsten 10 zurück
        private int tickCount = 0;
        private const int stepsPerDirection = 10;

        // Wird bei jedem Timer-Takt aufgerufen und bewegt jeden Stein um genau einen Schritt
        public void Bewegen()
        {
            bool outward = tickCount < stepsPerDirection;
            tickCount = (tickCount + 1) % (2 * stepsPerDirection);

            for (int i = 0; i < bricks.Count; i++)
            {
                if (outward)
                    bricks[i].Bewegen(Parameters.Direction.right);
                else
                    bricks[i].Bewegen(Parameters.Direction.left);
            }
        }
    }
}
