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
        public List<LegoBrick> Bricks => bricks;

        public ProductionLine(Pose pose)
        {
            x = pose.X;
            y = pose.Y;
            angle = pose.Angle;
        }

        // Erzeugt die Legosteine in der Farbe der Linie, einen auf jedem Platz
        public void CreateBricks()
        {
            Random rnd = new Random();

            for (int i = 0; i < Parameters.bricksPerLine; i++)
            {
                bricks.Add(new LegoBrick(PlatzPose(i), Parameters.LegoColor.blue, Parameters.LegoSize.small));
            }
        }

        // Position des Platzes Nummer "platz" auf dieser Linie (ohne die Pendelbewegung).
        // Die Plätze liegen nebeneinander entlang der Linie, direkt neben ihrer Innenseite,
        // und sind genauso gedreht wie die Linie.
        private Pose PlatzPose(int platz)
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
            int k = RichtungsIndex();
            double schrittLaenge = (k % 2 == 1) ? Parameters.step * Math.Sqrt(2) : Parameters.step;
            double fahrweg = stepsPerDirection * schrittLaenge;

            // Plätze so verteilen, dass die Steine auch am Ende des Fahrwegs noch auf der Linie liegen
            double abstand = (w - fahrweg) / count;
            double seitlich = h / 2 + brickSize / 2 + 3;

            double t = -w / 2 + abstand / 2 + platz * abstand;
            double brickMitteX = mitteX + ux * t + nx * seitlich;
            double brickMitteY = mitteY + uy * t + ny * seitlich;

            return new Pose((int)Math.Round(brickMitteX - brickSize / 2),
                            (int)Math.Round(brickMitteY - brickSize / 2),
                            angle);
        }

        // Legt einen Stein auf den nächsten freien Platz dieser Linie (z. B. auf der Recycle-Linie).
        // Gibt false zurück, wenn kein Platz mehr frei ist.
        public bool SteinAblegen(LegoBrick brick)
        {
            var (vx, vy) = Verschiebung();

            for (int platz = 0; platz < Parameters.bricksPerLine; platz++)
            {
                // Wo dieser Platz gerade liegt, inklusive Pendelbewegung
                Pose p = PlatzPose(platz);
                int px = p.X + vx;
                int py = p.Y + vy;

                bool belegt = bricks.Exists(b => Math.Abs(b.Heimat.X - px) <= 2 &&
                                                 Math.Abs(b.Heimat.Y - py) <= 2);
                if (belegt) continue;

                brick.NeueHeimat(new Pose(px, py, angle));
                bricks.Add(brick);
                return true;
            }
            return false;
        }

        // Wie weit die Steine gerade von ihrer Startposition weggependelt sind
        private (int dx, int dy) Verschiebung()
        {
            (int x, int y)[] einheit = { (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1) };
            int k = RichtungsIndex();

            // Schritte nach außen: steigt in den ersten 10 Takten bis 10, fällt dann wieder auf 0
            int schritte = tickCount <= stepsPerDirection ? tickCount : 2 * stepsPerDirection - tickCount;

            return (einheit[k].x * schritte * Parameters.step, einheit[k].y * schritte * Parameters.step);
        }

        // Linienwinkel auf die nächste der 8 Richtungen runden (0° = rechts, 45° = rechts unten, ...)
        private int RichtungsIndex()
        {
            return (int)Math.Round(((angle % 360) + 360) % 360 / 45) % 8;
        }

        // Ersetzt den Stein an Position index durch einen neuen Stein derselben Farbe.
        // Der neue Stein liegt genau dort, wo der alte auf der Linie liegen würde.
        /*public void NeuerStein(int index)
        {
            LegoBrick alt = bricks[index];
            var pose = new Pose(alt.Heimat.X, alt.Heimat.Y, alt.HeimatWinkel);
            bricks[index] = new LegoBrick(pose, Color, alt.Size);
        }*/

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

            int k = RichtungsIndex();

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
