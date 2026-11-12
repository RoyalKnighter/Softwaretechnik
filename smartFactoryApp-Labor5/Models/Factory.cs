using System;
using System.Collections.Generic;
using System.Linq;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;

namespace smartFactoryApp.ViewModels;

public class Factory
{

    public ProductionLineType orderFromLine;
    public LegoColor orderBrickColor;

    // Gibt es gerade einen Auftrag? Ohne Auftrag kann kein Stein gegriffen werden.
    public bool orderAktiv = false;

    private Robot robot;
    private ProductionLine productionRed;
    private ProductionLine productionBlue;
    private ProductionLine productionGreen;
    private ProductionLine productionYellow;

    // ABWEICHUNG vom UML: Die Recycle-Linie ist eine eigene ProductionLine (ohne Farbe, ohne Bewegung).
    // Im Diagramm gibt es dieses Feld nicht.
    private ProductionLine productionRecycle;

    // NEU (UML): Typ im Diagramm nicht sichtbar (kommt vermutlich mit der Klasse Logistics in Labor 5).
    // TODO: noch ungenutzt
    private object? logistics = null;

    // NEU (UML): Ergebnis der letzten Zufallsentscheidung beim Undock (true = zum Recycling)
    private bool naechsterZumRecycling = false;

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

    // Im UML eine Eigenschaft (vorher ein öffentliches Feld)
    public List<int> LegoOrder { get; } = new List<int>();
    public List<int> LegoDelivered { get; } = new List<int>();

    // Legosteine je Farbe
    public List<LegoBrick> RedBricks => productionRed.Bricks;
    public List<LegoBrick> BlueBricks => productionBlue.Bricks;
    public List<LegoBrick> GreenBricks => productionGreen.Bricks;
    public List<LegoBrick> YellowBricks => productionYellow.Bricks;

    // ABWEICHUNG vom UML: Steine auf der Recycle-Linie
    public List<LegoBrick> RecycleBricks => productionRecycle.Bricks;

    private Random random = new Random();

    public Factory()
    {
        robot = new Robot(RobotPose.Location, Borders);

        productionRed = new ProductionLine(RectRedPose, ProductionLineType.productionLineRed);
        productionBlue = new ProductionLine(RectBluePose, ProductionLineType.productionLineBlue);
        productionGreen = new ProductionLine(RectGreenPose, ProductionLineType.productionLineGreen);
        productionYellow = new ProductionLine(RectYellowPose, ProductionLineType.productionLineYellow);
        productionRecycle = new ProductionLine(RectRecyclePose, ProductionLineType.productionLineRecycle);

        LegoOrder.Add(0);
        LegoOrder.Add(0);
        LegoOrder.Add(0);
        LegoOrder.Add(0);

        LegoDelivered.Add(0);
        LegoDelivered.Add(0);
        LegoDelivered.Add(0);
        LegoDelivered.Add(0);
    }

    // NEU (UML): alle Steine auf allen Linien, inklusive Recycle-Linie
    private List<LegoBrick> AlleBricks()
    {
        return RedBricks.Concat(BlueBricks).Concat(GreenBricks).Concat(YellowBricks)
                        .Concat(RecycleBricks).ToList();
    }

    public void RobotBewegen(Direction dir)
    {
        robot.Bewegen(dir);

        // Greifen kann der Roboter nur den bestellten Stein von der bestellten Linie
        if (Beweger.Dockobjekt == null) robot.Kontakt(BestellteBricks());

        Beweger.DockobjektNachziehen(robot.Greifpunkt(), robot.Angle);
    }

    // Steine, die gerade gegriffen werden dürfen: richtige Farbe auf der richtigen Linie
    private List<LegoBrick> BestellteBricks()
    {
        if (!orderAktiv) return new List<LegoBrick>();

        ProductionLine linie = orderFromLine switch
        {
            ProductionLineType.productionLineRed    => productionRed,
            ProductionLineType.productionLineBlue   => productionBlue,
            ProductionLineType.productionLineGreen  => productionGreen,
            ProductionLineType.productionLineYellow => productionYellow,
            _                                       => productionRecycle
        };

        return linie.Bricks.Where(b => b.Color == orderBrickColor).ToList();
    }

    // Index einer Farbe in LegoOrder / LegoDelivered: 0 = rot, 1 = blau, 2 = grün, 3 = gelb
    private static int OrderIndex(LegoColor farbe)
    {
        return farbe switch
        {
            LegoColor.red   => 0,
            LegoColor.blue  => 1,
            LegoColor.green => 2,
            _               => 3
        };
    }

    public void LegoBricksBewegen()
    {
        productionRed.Bewegen();
        productionBlue.Bewegen();
        productionGreen.Bewegen();
        productionYellow.Bewegen();
        productionRecycle.Bewegen();

        // Angekommene Steine abholen
        foreach (LegoBrick brick in AlleBricks())
        {
            bool vonRecycleLinie = RecycleBricks.Contains(brick);

            if (brick.Status == BrickStatus.atCustomer)
            {
                if (vonRecycleLinie)
                {
                    // Nur Steine von der Recycle-Linie werden gelöscht
                    RecycleBricks.Remove(brick);
                }
                else
                {
                    // Stein zurück auf seine Produktionslinie
                    brick.RecyclePlatz = -1;
                    brick.Status = BrickStatus.onLine;
                    brick.Bewegen(Direction.stop);   // legt ihn sofort auf seinen Platz
                }
            }
            else if (brick.Status == BrickStatus.atRecycling)
            {
                if (!vonRecycleLinie)
                {
                    // Neuer Stein in derselben Farbe auf den reservierten Platz der Recycle-Linie
                    var neu = new LegoBrick(RecyclePlatzPose(brick.RecyclePlatz), brick.Color, brick.Size);
                    neu.RecyclePlatz = brick.RecyclePlatz;
                    neu.Status = BrickStatus.atRecycleLine;
                    RecycleBricks.Add(neu);

                    brick.RecyclePlatz = -1;
                }

                // Der Stein selbst geht zurück auf seine Produktionslinie
                brick.Status = BrickStatus.onLine;
                brick.Bewegen(Direction.stop);
            }
        }
    }

    // NEU (UML): erster freier Platz auf der Recycle-Linie, oder -1 wenn alle belegt sind.
    // Plätze von Steinen, die gerade zum Recycling unterwegs sind, zählen als belegt.
    private int FreierRecyclePlatz()
    {
        var belegt = AlleBricks()
            .Where(b => RecycleBricks.Contains(b) ||
                        b.Status == BrickStatus.toRecycling || b.Status == BrickStatus.atRecycling)
            .Select(b => b.RecyclePlatz)
            .ToHashSet();

        for (int platz = 0; platz < bricksPerLine; platz++)
        {
            if (!belegt.Contains(platz)) return platz;
        }
        return -1;
    }

    // NEU (UML): Position eines Platzes auf der Recycle-Linie.
    // Die Linie wird in gleich große Plätze aufgeteilt. Der Stein liegt in der Mitte seines Platzes,
    // aber seitlich neben der Linie (wie bei den Produktionslinien), damit der Roboter ihn greifen kann.
    private Pose RecyclePlatzPose(int platz)
    {
        const double brickSize = 12;
        double w = ProductionLineWidth;
        double h = ProductionLineHeight;

        // Mitte der Linie, Richtung entlang der Linie (u) und quer dazu (n)
        double mitteX = RectRecyclePose.X + w / 2;
        double mitteY = RectRecyclePose.Y + h / 2;
        double rad = RectRecyclePose.Angle * Math.PI / 180;
        double ux = Math.Cos(rad), uy = Math.Sin(rad);
        double nx = -uy, ny = ux;

        double t = -w / 2 + (platz + 0.5) * w / bricksPerLine;   // Position entlang der Linie
        double seitlich = h / 2 + brickSize / 2 + 3;             // Abstand von der Mitte der Linie

        return new Pose((int)Math.Round(mitteX + ux * t + nx * seitlich - brickSize / 2),
                        (int)Math.Round(mitteY + uy * t + ny * seitlich - brickSize / 2),
                        RectRecyclePose.Angle);
    }

    public UndockResult Undock()
    {
        if (Beweger.Dockobjekt is not LegoBrick brick) return UndockResult.notDocked;
        if (!Beweger.Undock()) return UndockResult.notOnDeliveryLine;

        // Steine von der Recycle-Linie gehen immer zum Kunden und werden dort gelöscht.
        // Alle anderen gehen mit 50 % Wahrscheinlichkeit zum Recycling, wenn dort ein Platz frei ist.
        bool vonRecycleLinie = RecycleBricks.Contains(brick);
        int platz = vonRecycleLinie ? -1 : FreierRecyclePlatz();

        naechsterZumRecycling = platz >= 0 && Random.Shared.Next(2) == 0;

        UndockResult ergebnis;
        if (naechsterZumRecycling)
        {
            brick.ZumRecycling(platz, RecyclePlatzPose(platz));
            ergebnis = UndockResult.toRecycling;

            // Derselbe Auftrag geht weiter, der Stein muss jetzt von der Recycle-Linie geholt werden
            orderFromLine = ProductionLineType.productionLineRecycle;
        }
        else
        {
            brick.ZumKunden();
            ergebnis = UndockResult.toCustomer;

            // Stein ist geliefert: zählen und den nächsten Auftrag erzeugen
            LegoDelivered[OrderIndex(brick.Color)]++;
            CreateOrder();
        }

        // Den Roboter nach dem Undocken etwas zurückfahren, weg vom abgelegten Stein
        for (int i = 0; i < 4; i++)
            robot.Bewegen(Direction.down);

        return ergebnis;
    }

    public void SetUPProductionLines()
    {
        productionRed.CreateBricks();
        productionBlue.CreateBricks();
        productionGreen.CreateBricks();
        productionYellow.CreateBricks();
    }

    public void GoToDelivery()
    {
        robot.GoToDelivery();
        Beweger.DockobjektNachziehen(robot.Greifpunkt(), robot.Angle);
    }

    // Erzeugt eine neue Bestellung über insgesamt 8 Steine.
    // Von jeder Farbe wird höchstens so viel bestellt, wie es Steine dieser Farbe auf allen Linien gibt.
    public void Order()
    {
        // Neue Bestellung: alte Werte zurücksetzen
        for (int i = 0; i < LegoOrder.Count; i++)
        {
            LegoOrder[i] = 0;
            LegoDelivered[i] = 0;
        }

        // Wie viele Steine gibt es von jeder Farbe insgesamt auf den Produktionslinien?
        // (Die Recycle-Linie zählt nicht, weil CreateOrder dort nie einen Stein anfordert.)
        List<LegoBrick> alle = RedBricks.Concat(BlueBricks).Concat(GreenBricks).Concat(YellowBricks).ToList();
        int[] vorhanden = new int[4];

        vorhanden[0] = alle.Count(b => b.Color == LegoColor.red);
        vorhanden[1] = alle.Count(b => b.Color == LegoColor.blue);
        vorhanden[2] = alle.Count(b => b.Color == LegoColor.green);
        vorhanden[3] = alle.Count(b => b.Color == LegoColor.yellow);

        // 8 Steine bestellen, jeweils eine zufällige Farbe, von der noch genug da ist
        for (int n = 0; n < 8; n++)
        {
            List<int> moeglich = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (LegoOrder[i] < vorhanden[i])
                    moeglich.Add(i);
            }

            if (moeglich.Count == 0) break;   // es gibt insgesamt weniger als 8 Steine

            LegoOrder[moeglich[random.Next(moeglich.Count)]]++;
        }

        CreateOrder();
    }

    // Wählt zufällig eine offene Bestellung und eine Produktionslinie, auf der ein greifbarer
    // Stein in dieser Farbe liegt. Die Recycle-Linie wird dabei nie gewählt.
    // Setzt am Ende orderBrickColor, orderFromLine und orderAktiv.
    public void CreateOrder()
    {
        // Nur die farbigen Produktionslinien, nie die Recycle-Linie
        var linien = new List<ProductionLine> { productionRed, productionBlue, productionGreen, productionYellow };

        // Alle möglichen Kombinationen aus offener Farbe und Linie sammeln
        var moeglich = new List<(LegoColor farbe, ProductionLineType typ)>();

        for (int i = 0; i < LegoOrder.Count; i++)
        {
            // Nur offene Bestellungen: bestellt und noch nicht komplett geliefert
            if (LegoOrder[i] <= 0 || LegoOrder[i] <= LegoDelivered[i]) continue;

            LegoColor farbe = i switch
            {
                0 => LegoColor.red,
                1 => LegoColor.blue,
                2 => LegoColor.green,
                _ => LegoColor.yellow
            };

            foreach (ProductionLine linie in linien)
            {
                bool hatStein = linie.Bricks.Any(b => b.Color == farbe && b.Status == BrickStatus.onLine);

                if (hatStein)
                    moeglich.Add((farbe, linie.LineType));
            }
        }

        // Keine Bestellung erfüllbar (oder alles geliefert): kein Auftrag
        if (moeglich.Count == 0)
        {
            orderAktiv = false;
            return;
        }

        // Eine Kombination zufällig auswählen und am Ende setzen
        var gewaehlt = moeglich[random.Next(moeglich.Count)];
        orderBrickColor = gewaehlt.farbe;
        orderFromLine = gewaehlt.typ;
        orderAktiv = true;
    }

    public void GoToPickUP() => robot.GoToPickUP(orderFromLine);
}
