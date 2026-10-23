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
        KontaktPruefen();
    }

    public void LegoBricksBewegen()
    {
        productionLine.Bewegen();
        KontaktPruefen();
    }

    // Prüft für jeden Legostein, ob der Roboter ihn berührt; ein berührter Stein wird automatisch angedockt
    private void KontaktPruefen()
    {
        foreach (var brick in productionLine.Bricks)
            robot.Kontakt(brick);
    }

    public void Undock()
    {
        if (robot.Undock())
        {
            // Den Roboter nach dem Undocken etwas zurückfahren, damit er den Stein nicht sofort wieder dockt
            for (int i = 0; i < 4; i++)
                robot.Bewegen(Direction.down);
        }
    }
}
