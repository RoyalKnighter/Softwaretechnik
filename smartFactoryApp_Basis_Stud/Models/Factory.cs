using System.Collections.Generic;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models;

public class Factory
{
    Robot robot;
    ProductionLine productionLine;

    // Properties to expose the robot's position to the ViewModel
    public double RobotX => robot.Location.X;
    public double RobotY => robot.Location.Y;

    // Property to expose the Lego bricks of the Production Line to the ViewModel
    public IReadOnlyList<LegoBrick> Bricks => productionLine.Bricks;

    public Factory()
    {
        robot = new Robot(ViewParameters.RobotPosition, ViewParameters.Borders);
        productionLine = new ProductionLine(ViewParameters.ProductionLinePosition, LegoColor.red);
    }

    public void RobotBewegen(Direction dir)
    {
        robot.Bewegen(dir);
       
    }

    public void LegoBricksBewegen()
    {
        productionLine.Bewegen();
       
    }

    // Prüft für jeden Legostein, ob der Roboter ihn berührt; ein berührter Stein wird automatisch angedockt
    

    public void Undock()
    {
        
    }
}
