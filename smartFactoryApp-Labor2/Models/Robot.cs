using System.Drawing;
namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
        Location = location;
        Borders = borders;
        BodyWidth = ViewParameters.RobotWidth;
        BodyHeight = ViewParameters.RobotHeight;
    }
    public Robot(Point location) : base(location)
    {
        Location = location;
        BodyWidth = ViewParameters.RobotWidth;
        BodyHeight = ViewParameters.RobotHeight;
    }
    public override void Bewegen(Parameters.Direction direction)
    {
        base.Bewegen(direction);
    }
}