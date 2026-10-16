using System.Drawing;

namespace smartFactoryApp.Models
{
    internal class Robot : Beweger
    {
        public Robot(Point location, Rectangle borders) : base(location, borders)
        {
            Step = Parameters.robotStep;
        }
    }
}
