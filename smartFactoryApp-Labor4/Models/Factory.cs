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

    // Steine, die auf der Recycle-Linie liegen (höchstens maxRecycleBricks)
    private List<LegoBrick> recycleBricks = new List<LegoBrick>();
    private Random zufall = new Random();

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
    public IReadOnlyList<LegoBrick> RecycleBricks => recycleBricks;

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

        AngekommeneSteineAbholen(productionRed);
        AngekommeneSteineAbholen(productionBlue);
        AngekommeneSteineAbholen(productionGreen);
        AngekommeneSteineAbholen(productionYellow);
    }

    public void Undock()
    {
        LegoBrick? brick = Beweger.Undock();
        if (brick == null) return;

        // Mit 50 % Wahrscheinlichkeit zum Recycling, aber nur, wenn auf der
        // Recycle-Linie noch Platz ist (Steine, die gerade unterwegs sind, zählen mit)
        int unterwegs = RedBricks.Concat(BlueBricks).Concat(GreenBricks).Concat(YellowBricks)
                                 .Count(b => b.Ausgeliefert && !b.ZumKunden);
        bool platzFrei = recycleBricks.Count + unterwegs < maxRecycleBricks;

        brick.ZumKunden = !platzFrei || zufall.Next(2) == 0;

        // Den Roboter nach dem Undocken etwas zurückfahren, weg vom abgelegten Stein
        for (int i = 0; i < 4; i++)
            robot.Bewegen(Direction.down);
    }

    // Steine, die am Ende der Delivery-Linie angekommen sind, werden abgeholt:
    // Zum Kunden -> verschwindet, zum Recycling -> kommt auf die Recycle-Linie.
    // In beiden Fällen bekommt die Produktionslinie einen neuen Stein.
    private void AngekommeneSteineAbholen(ProductionLine linie)
    {
        for (int i = 0; i < linie.Bricks.Count; i++)
        {
            LegoBrick brick = linie.Bricks[i];
            if (!brick.AmZiel) continue;

            if (!brick.ZumKunden)
                AufRecycleLinieLegen(brick);

            linie.NeuerStein(i);
        }
    }

    // Legt den Stein auf den nächsten freien Platz der Recycle-Linie
    private void AufRecycleLinieLegen(LegoBrick brick)
    {
        int platz = recycleBricks.Count;
        double w = ProductionLineWidth;
        double h = ProductionLineHeight;

        // Mitte der Recycle-Linie und Richtung entlang der Linie
        double mitteX = RectRecyclePose.X + w / 2;
        double mitteY = RectRecyclePose.Y + h / 2;
        double rad = RectRecyclePose.Angle * Math.PI / 180;

        // Die Linie in gleich große Plätze aufteilen, der Stein kommt in die Mitte seines Platzes
        double t = -w / 2 + (platz + 0.5) * w / maxRecycleBricks;

        brick.X = (int)Math.Round(mitteX + Math.Cos(rad) * t - 6);
        brick.Y = (int)Math.Round(mitteY + Math.Sin(rad) * t - 6);
        brick.Angle = RectRecyclePose.Angle;
        brick.Ausgeliefert = false;
        brick.AmZiel = false;

        recycleBricks.Add(brick);
    }
}
