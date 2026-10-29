using System.Collections.Generic;
using System.Linq;
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
    public List<LegoBrick> RedBricks => (List<LegoBrick>) productionRed.Bricks;
    public List<LegoBrick> BlueBricks => (List<LegoBrick>) productionBlue.Bricks;
    public List<LegoBrick> GreenBricks => (List<LegoBrick>) productionGreen.Bricks;
    public List<LegoBrick> YellowBricks => (List<LegoBrick>) productionYellow.Bricks;

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
        
        if (Beweger.dockObjekt == null) robot.Kontakt(RedBricks.Concat(BlueBricks).Concat(GreenBricks).Concat(YellowBricks).ToList());

        Beweger.DockobjektNachziehen(robot.GreifPunkt(), robot.Angle);

    }

    public void LegoBricksBewegen()
    {
        productionRed.Bewegen();
        productionBlue.Bewegen();
        productionGreen.Bewegen();
        productionYellow.Bewegen();
    }

    public void Undock() => Beweger.Undock();
}
