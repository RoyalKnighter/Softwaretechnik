using System.Collections.Generic;
using System.Reflection.Metadata;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;

namespace smartFactoryApp.ViewModels;

public class Factory
{
    private Robot robot;
    private ProductionLine productionRed;
    private ProductionLine productionBlue;
    private ProductionLine productionGreen;
    private ProductionLine productionYellow;

    // Roboter
    public double RobotX => robot.X;
    public double RobotY => robot.Y;
    public Direction RobotDirection
    {
        get => robot.RobotDirection;
        set => robot.RobotDirection = value;
    }
    public double RobotAngle
    {
        get => robot.Angle;
        set => robot.Angle = value;
    }

    public List<int> LegoOrder = new List<int>();

    // Legosteine je Farbe
    public IReadOnlyList<LegoBrick> RedBricks => productionRed.Bricks;
    public IReadOnlyList<LegoBrick> BlueBricks => productionBlue.Bricks;
    public IReadOnlyList<LegoBrick> GreenBricks => productionGreen.Bricks;
    public IReadOnlyList<LegoBrick> YellowBricks => productionYellow.Bricks;

    public Factory()
    {
        robot = new Robot(RobotPose.Location, Borders);

        productionRed    = new ProductionLine(RectRedPose,    LegoColor.red);
        productionBlue   = new ProductionLine(RectBluePose,   LegoColor.blue);
        productionGreen  = new ProductionLine(RectGreenPose,  LegoColor.green);
        productionYellow = new ProductionLine(RectYellowPose, LegoColor.yellow);

        LegoOrder.Add(0);
        LegoOrder.Add(0);
        LegoOrder.Add(0);
        LegoOrder.Add(0);
    }

    public void RobotBewegen(Direction dir)
    {
        robot.Bewegen(dir);
    }

    public void LegoBricksBewegen()
    {
        productionRed.Bewegen();
        productionBlue.Bewegen();
        productionGreen.Bewegen();
        productionYellow.Bewegen();
    }

    public void Undock()
    {
        
    }
}
