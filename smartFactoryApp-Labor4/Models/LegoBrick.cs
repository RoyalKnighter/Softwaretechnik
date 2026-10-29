using System.Drawing;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models
{
    public class LegoBrick : Beweger
    {
        private bool gedockt;
        private bool ausgeliefert = false;

        private Point heimat;
        private readonly double heimatWinkel;

        public bool Gedockt { get { return gedockt; } set { gedockt = value; } }
        public bool Ausgeliefert { get => ausgeliefert; set => ausgeliefert = value; }

        // Fährt der ausgelieferte Stein nach rechts zum Kunden (true) oder nach links zum Recycling (false)?
        public bool ZumKunden { get; set; } = true;

        // Der ausgelieferte Stein ist am Ende der Delivery-Linie angekommen.
        // Die Factory holt ihn dann ab und legt einen neuen Stein auf die Produktionslinie.
        public bool AmZiel { get; set; } = false;

        // Wo ein neuer Stein auf der Produktionslinie hingelegt werden muss
        public Point Heimat => heimat;
        public double HeimatWinkel => heimatWinkel;
        public LegoColor Color { get; set; }
        public LegoSize Size { get; set; }

        public LegoBrick(Pose pose, LegoColor color, LegoSize size) : base(pose, ViewParameters.BrickBorders)
        {
            Color = color;
            Size = size;
            heimat = pose.Location;
            heimatWinkel = pose.Angle;
        }

        public override string ToString()
        {
            return $"{Color} {Size} LegoBrick bei ({Location.X}, {Location.Y}), {Angle}°";
        }

        public override void Bewegen(Direction direction)
        {
            // Heimat einen Schritt mit der Produktionslinie mitbewegen
            Point aktuell = Location;
            Location = heimat;
            base.Bewegen(direction);
            heimat = Location;
            Location = aktuell;

            // Ein gedockter Stein wird nur noch vom Roboter bewegt,
            // ein angekommener Stein wartet darauf, von der Factory abgeholt zu werden
            if (Gedockt || AmZiel) return;

            if (!Ausgeliefert)
            {
                // Stein liegt auf seiner Produktionslinie
                Location = heimat;
                return;
            }

            // Ausgelieferter Stein: auf der Delivery-Linie nach rechts zum Kunden
            // oder nach links zum Recycling fahren, bis er hinter dem Ende der Linie ist
            int deliveryLinks  = ViewParameters.RectDeliveryPose.X;
            int deliveryRechts = deliveryLinks + ViewParameters.ProductionDeliveryWidth;

            if (ZumKunden)
            {
                if (X <= deliveryRechts) base.Bewegen(Direction.right);
                else AmZiel = true;
            }
            else
            {
                if (X + 12 >= deliveryLinks) base.Bewegen(Direction.left);
                else AmZiel = true;
            }
        }
    }
}
