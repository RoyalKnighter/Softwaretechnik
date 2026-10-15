using System.Drawing;
namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
        Location = location;
        Borders = borders;
    }
    public Robot(Point location) : base(location)
    {
        Location = location;
    }
    public override void Bewegen(Parameters.Direction direction)
    {
        base.Bewegen(direction);
    }
}

