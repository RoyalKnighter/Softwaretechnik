using System.Drawing;
namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
        Location = location;
        Borders = borders;
        BodyWidth = 70;
        BodyHeight = 50;
    }
    public Robot(Point location) : base(location)
    {
        Location = location;
        BodyWidth = 70;
        BodyHeight = 50;
    }
    public override void Bewegen(Parameters.Direction direction)
    {
        base.Bewegen(direction);
    }
}