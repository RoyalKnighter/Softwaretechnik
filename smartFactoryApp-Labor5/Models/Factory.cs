using System;
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
    private ProductionLine productionRecycle;

    private Random rnd = new Random();

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
    public List<int> LegoDelivered = new List<int>();

    // Legosteine je Farbe
    public List<LegoBrick> RedBricks => productionRed.Bricks;
    public List<LegoBrick> BlueBricks => productionBlue.Bricks;
    public List<LegoBrick> GreenBricks => productionGreen.Bricks;
    public List<LegoBrick> YellowBricks => productionYellow.Bricks;
    public List<LegoBrick> RecycleBricks => productionRecycle.Bricks;

    public Factory()
    {
        robot = new Robot(RobotPose.Location, Borders);

        productionRed    = new ProductionLine(RectRedPose);
        productionBlue   = new ProductionLine(RectBluePose);
        productionGreen  = new ProductionLine(RectGreenPose);
        productionYellow = new ProductionLine(RectYellowPose);
        productionRecycle = new ProductionLine(RectRecyclePose);

        LegoOrder.Add(0);
        LegoOrder.Add(0);
        LegoOrder.Add(0);
        LegoOrder.Add(0);
        
        LegoDelivered.Add(0);
        LegoDelivered.Add(0);
        LegoDelivered.Add(0);
        LegoDelivered.Add(0);
    }

    public void RobotBewegen(Direction dir)
    {
        robot.Bewegen(dir);
        
        if (Beweger.dockObjekt == null) robot.Kontakt(RedBricks.Concat(BlueBricks).Concat(GreenBricks).Concat(YellowBricks).Concat(RecycleBricks).ToList());

        Beweger.DockobjektNachziehen(robot.GreifPunkt(), robot.Angle);

    }

    public void LegoBricksBewegen()
    {
        productionRed.Bewegen();
        productionBlue.Bewegen();
        productionGreen.Bewegen();
        productionYellow.Bewegen();
        productionRecycle.Bewegen();

        AngekommeneSteineAbholen(productionRed);
        AngekommeneSteineAbholen(productionBlue);
        AngekommeneSteineAbholen(productionGreen);
        AngekommeneSteineAbholen(productionYellow);
        AngekommeneSteineAbholen(productionRecycle);
    }

    public void Undock()
    {
        LegoBrick? brick = Beweger.Undock();
        if (brick == null) return;

        // Mit 50 % Wahrscheinlichkeit zum Recycling, aber nur, wenn auf der
        // Recycle-Linie noch Platz ist. Steine, die gerade zum Recycling unterwegs sind, zählen mit.
        int unterwegs = RedBricks.Concat(BlueBricks).Concat(GreenBricks).Concat(YellowBricks)
                                 .Count(b => b.Ausgeliefert && !b.ZumKunden);
        int belegt = RecycleBricks.Count + unterwegs;

        // Kommt der Stein selbst von der Recycle-Linie, wird sein Platz dort gleich frei
        if (RecycleBricks.Contains(brick)) belegt--;

        bool platzFrei = belegt < maxRecycleBricks;

        brick.ZumKunden = !platzFrei || rnd.Next(2) == 0;

        // Den Roboter nach dem Undocken etwas zurückfahren, weg vom abgelegten Stein
        for (int i = 0; i < 4; i++)
            robot.Bewegen(Direction.down);
    }

    // Steine, die am Ende der Delivery-Linie angekommen sind, werden abgeholt:
    // Zum Kunden -> verschwindet, zum Recycling -> kommt auf die Recycle-Linie.
    // Kommt der Stein von einer farbigen Linie, bekommt sie einen neuen Stein.
    // Kommt er von der Recycle-Linie, wird er dort nur entfernt.
    private void AngekommeneSteineAbholen(ProductionLine linie)
    {
        // Rückwärts, weil Steine aus der Liste entfernt werden können
        for (int i = linie.Bricks.Count - 1; i >= 0; i--)
        {
            LegoBrick brick = linie.Bricks[i];
            if (!brick.AmZiel) continue;

            if (linie == productionRecycle)
                linie.Bricks.RemoveAt(i);
            else
                brick.Location = brick.Heimat;
                //linie.NeuerStein(i);     // muss vor SteinAblegen kommen, weil es die alte Heimat braucht

            if (!brick.ZumKunden)
                productionRecycle.SteinAblegen(brick);
        }
    }

    public void Order()
    {
        for (int i = 0; i < 8; i++)
            LegoOrder[rnd.Next(4)]++;
    }

    public void GoToDelivery()
    {
        robot.GoToDelivery();
        Beweger.DockobjektNachziehen(robot.GreifPunkt(), robot.Angle);
    }

    public void SetUPProductionLines()
    {
        productionRed.CreateBricks();
        productionBlue.CreateBricks();
        productionGreen.CreateBricks();
        productionYellow.CreateBricks();
    }
}
