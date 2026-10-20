using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models
{
    public class LegoBrick : Beweger
    {
        private bool gedockt;
        public bool Gedockt { get {return gedockt; } set { gedockt = value; } }
        public LegoColor Color { get; set;}
        public LegoSize Size { get; set; }

        public LegoBrick(Pose pose, LegoColor color, LegoSize size) : base(pose, ViewParameters.BrickBorders)
        {
            Color = color;
            Size = size;
        }

        public override string ToString()
        {
            return $"{Color} {Size} LegoBrick bei ({Location.X}, {Location.Y}), {Angle}°";
        }

        public override void Bewegen(Direction direction)
        {
            if (!Gedockt)
            {
                base.Bewegen(direction);
            }
        }
    }
}
