using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Controls.Mixins;

namespace smartFactoryApp.Models
{
    // Ein einzelner Legostein
    public class LegoBrick : Beweger
    {
        private Point originalLocation;
        private bool ausgeliefert = false;
        public bool Ausgeliefert { get => ausgeliefert; set => ausgeliefert = value; }
        public Parameters.LegoColor Color { get; }
        bool gedockt = false;
        public bool Gedockt { get => gedockt; set => gedockt = value; }

        public LegoBrick(Point location, Parameters.LegoColor color) :
                                                 base(location, ViewParameters.BrickBorders)
        {
            originalLocation = location;
            Color = color;
        }

        // Ein gedockter Stein wird nur noch vom Roboter bewegt
        public override void Bewegen(Parameters.Direction dir)
        {
            if (Gedockt) return;

            if (!Ausgeliefert) base.Bewegen(dir);
            else
            {
                if (Location.X - 10 <= deliveryLine.Right) base.Bewegen(Parameters.Direction.right);
                else
                {
                    Ausgeliefert = false;
                    Location = originalLocation;
                }
            }
        }

        public override string ToString()
        {
            return $"{Color}";
        }
    }
}
