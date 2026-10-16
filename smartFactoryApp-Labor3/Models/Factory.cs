using System.Collections.Generic;
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
    public double RobotAngle
    {
        get => robot.Angle;
        set => robot.Angle = value;
    }

    // Produktionslinien
    public double ProductionRedX => productionRed.Location.X;
    public double ProductionRedY => productionRed.Location.Y;
    public double ProductionRedAngle => productionRed.Angle;

    public double ProductionBlueX => productionBlue.Location.X;
    public double ProductionBlueY => productionBlue.Location.Y;
    public double ProductionBlueAngle => productionBlue.Angle;

    public double ProductionGreenX => productionGreen.Location.X;
    public double ProductionGreenY => productionGreen.Location.Y;
    public double ProductionGreenAngle => productionGreen.Angle;

    public double ProductionYellowX => productionYellow.Location.X;
    public double ProductionYellowY => productionYellow.Location.Y;
    public double ProductionYellowAngle => productionYellow.Angle;

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
}
