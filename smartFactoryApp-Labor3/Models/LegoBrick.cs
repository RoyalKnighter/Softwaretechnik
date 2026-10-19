using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models
{
    public class LegoBrick : Beweger
    {
        private bool gedockt;
        private LegoColor color;
        private LegoSize size;
        public bool Gedockt { get {return gedockt; } set { gedockt = value; } }
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

        public override void Bewegen(Direction direction)
        {
            if (!Gedockt)
            {
                double s = step;
                double dx = 0, dy = 0;

                switch (direction)
                {
                    case Direction.up:        dy = -s;          break;
                    case Direction.down:      dy =  s;          break;
                    case Direction.left:      dx = -s;          break;
                    case Direction.right:     dx =  s;          break;
                    case Direction.upLeft:    dx = -s; dy = -s; break;
                    case Direction.upRight:   dx =  s; dy = -s; break;
                    case Direction.downLeft:  dx = -s; dy =  s; break;
                    case Direction.downRight: dx =  s; dy =  s; break;
                    default: return;
                }

                if (Borders.IsEmpty)
                {
                    X += dx;
                    Y += dy;
                    return;
                }

                if (X + dx >= Borders.Left && X + dx <= Borders.Right)
                    X += dx;
                if (Y + dy >= Borders.Top && Y + dy <= Borders.Bottom)
                    Y += dy;
            }
        }
    }
}
