using System.Drawing;

namespace smartFactoryApp.Models
{
    // Ein einzelner Legostein
    public class LegoBrick : Beweger
    {
        public Parameters.LegoColor Color { get; }

        public LegoBrick(Point location, Parameters.LegoColor color) :
                                                 base(location, ViewParameters.BrickBorders)
        {
            Color = color;
        }

        // Ein gedockter Stein wird nur noch vom Roboter bewegt
        public override void Bewegen(Parameters.Direction dir)
        {
            base.Bewegen(dir);
        }

        public override string ToString()
        {
            return $"{Color}";
        }
    }
}
