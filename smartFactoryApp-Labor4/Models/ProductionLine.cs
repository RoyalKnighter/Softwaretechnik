using System;
using System.Collections.Generic;
using System.Drawing;

namespace smartFactoryApp.Models
{
    public class ProductionLine
    {
        private int x;          // linke obere Ecke des ungedrehten Bildes
        private int y;
        private double angle;   // Drehung um die Bildmitte, in Grad

        // Die Legosteine, die auf dieser Produktionslinie liegen
        private List<LegoBrick> bricks = new List<LegoBrick>();

        // Zähler für die Timer-Takte: die ersten 10 Takte hin, die nächsten 10 zurück
        private int tickCount = 0;
        private const int stepsPerDirection = 10;

        public Point Location => new Point(x, y);
        public double Angle => angle;
        public Parameters.LegoColor Color { get; }
        public IReadOnlyList<LegoBrick> Bricks => bricks;

        public ProductionLine(Pose pose, Parameters.LegoColor color)
        {
            x = pose.X;
            y = pose.Y;
            angle = pose.Angle;
            Color = color;
            CreateBricks();
        }
        public ProductionLine(Point location, double angle, Parameters.LegoColor color)
            : this(new Pose(location.X, location.Y, angle), color)
        {
        }
        public ProductionLine(Point location, Parameters.LegoColor color)
            : this(new Pose(location.X, location.Y, 0), color)
        {
        }

        // Erzeugt die Legosteine in der Farbe der Linie. Sie liegen nebeneinander
        // entlang der Linie, direkt neben ihrer Innenseite, und sind genauso gedreht wie die Linie.
        private void CreateBricks()
        {
            const double brickSize = 12;
            int count = Parameters.bricksPerLine;
            double w = ViewParameters.ProductionLineWidth;
            double h = ViewParameters.ProductionLineHeight;

            // Mittelpunkt der Linie (um ihn wird das Bild gedreht)
            double mitteX = x + w / 2;
            double mitteY = y + h / 2;

            // Richtung entlang der Linie (u) und quer dazu (n)
            double rad = angle * Math.PI / 180;
            double ux = Math.Cos(rad), uy = Math.Sin(rad);
            double nx = -uy, ny = ux;

            // So weit fahren die Steine in 10 Takten (schräg ist ein Schritt √2 mal länger)
            int k = (int)Math.Round(((angle % 360) + 360) % 360 / 45) % 8;
            double schrittLaenge = (k % 2 == 1) ? Parameters.step * Math.Sqrt(2) : Parameters.step;
            double fahrweg = stepsPerDirection * schrittLaenge;

            // Steine so verteilen, dass sie auch am Ende des Fahrwegs noch auf der Linie liegen
            double abstand = (w - fahrweg) / count;
            double seitlich = h / 2 + brickSize / 2 + 3;

            for (int i = 0; i < count; i++)
            {
                double t = -w / 2 + abstand / 2 + i * abstand;
                double brickMitteX = mitteX + ux * t + nx * seitlich;
                double brickMitteY = mitteY + uy * t + ny * seitlich;

                var pose = new Pose((int)Math.Round(brickMitteX - brickSize / 2),
                                    (int)Math.Round(brickMitteY - brickSize / 2),
                                    angle);
                bricks.Add(new LegoBrick(pose, Color, Parameters.LegoSize.small));
            }
        }

        // Ersetzt den Stein an Position index durch einen neuen Stein derselben Farbe.
        // Der neue Stein liegt genau dort, wo der alte auf der Linie liegen würde.
        public void NeuerStein(int index)
        {
            LegoBrick alt = bricks[index];
            var pose = new Pose(alt.Heimat.X, alt.Heimat.Y, alt.HeimatWinkel);
            bricks[index] = new LegoBrick(pose, Color, alt.Size);
        }

        // Wird bei jedem Timer-Takt aufgerufen und bewegt jeden Stein um genau einen Schritt
        // entlang der Linie: 10 Takte hin, 10 Takte zurück
        public void Bewegen()
        {
            Parameters.Direction[] richtungen =
            {
                Parameters.Direction.right,   Parameters.Direction.downRight,
                Parameters.Direction.down,    Parameters.Direction.downLeft,
                Parameters.Direction.left,    Parameters.Direction.upLeft,
                Parameters.Direction.up,      Parameters.Direction.upRight
            };

            // Linienwinkel auf die nächste der 8 Richtungen runden (0° = rechts, 45° = rechts unten, ...)
            int k = (int)Math.Round(((angle % 360) + 360) % 360 / 45) % 8;

            bool outward = tickCount < stepsPerDirection;
            tickCount = (tickCount + 1) % (2 * stepsPerDirection);

            Parameters.Direction richtung = outward ? richtungen[k] : richtungen[(k + 4) % 8];

            foreach (LegoBrick brick in bricks)
            {
                brick.Bewegen(richtung);
            }
        }
    }
}
