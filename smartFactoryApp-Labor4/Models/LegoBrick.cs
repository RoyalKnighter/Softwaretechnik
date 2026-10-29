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

            // Ein gedockter Stein wird nur noch vom Roboter bewegt
            if (Gedockt) return;

            if (!Ausgeliefert)
            {
                // Stein liegt auf seiner Produktionslinie
                Location = heimat;
                return;
            }

            // Ausgelieferter Stein: auf der Delivery-Linie nach rechts fahren,
            // bis er hinter ihrem Ende ist, dann zurück auf seine Produktionslinie
            int deliveryRechts = ViewParameters.RectDeliveryPose.X + ViewParameters.ProductionDeliveryWidth;

            if (X <= deliveryRechts)
            {
                base.Bewegen(Direction.right);
            }
            else
            {
                Ausgeliefert = false;
                Location = heimat;
                Angle = heimatWinkel;
            }
        }
    }
}
