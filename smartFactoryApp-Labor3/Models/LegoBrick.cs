using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models
{
    public class LegoBrick : Beweger
    {
        private LegoColor color;
        private LegoSize size;
        public LegoColor Color { get { return color; } set { color = value; } }
        public LegoSize Size { get { return size; } set { size = value; } }

        public LegoBrick(Pose pose, LegoColor color, LegoSize size) : base(pose, ViewParameters.BrickBorders)
        {
            Color = color;
            Size = size;
        }

        public override string ToString()
        {
            return $"{Color} {Size} LegoBrick bei ({Location.X}, {Location.Y}), {Angle}°";
        }
    }
}
