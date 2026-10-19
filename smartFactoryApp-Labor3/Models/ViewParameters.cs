using System.Drawing;

namespace smartFactoryApp.Models;

public struct Pose
{
    public int X { get; }
    public int Y { get; }
    public double Angle { get; }
    public Pose(int x, int y, double angle)
    {
        X = x;
        Y = y;
        Angle = angle;
    }
    public Pose(int x, int y)
    {
        X = x;
        Y = y;
        Angle = 0;
    }
    public Point Location => new Point(X, Y);
}

public static class ViewParameters
{
    public static Pose RobotPose = new Pose(300, 200, 0);

    // Bereich, in dem die linke obere Ecke des (ungedrehten) Roboters liegen darf.
    // Die Wand liegt also bei Right + Roboterbreite bzw. Bottom + Roboterhöhe (≈ 559 / 567).
    public static Rectangle Borders = new Rectangle(33, 38, 465, 475);

    // Dasselbe für die 12x12 großen Legosteine
    public static Rectangle BrickBorders = new Rectangle(37, 37, 510, 518);

    // Größe des Bildes einer Produktionslinie (ungedreht)
    public const int ProductionLineWidth = 100;
    public const int ProductionLineHeight = 22;

    public const int ProductionDeliveryWidth = 400;
    public const int ProductionDeliveryHeight = 60;

    // Linke obere Ecke und Drehwinkel der vier Produktionslinien
    public static Pose RectRedPose    = new Pose(50,  150,  270);
    public static Pose RectBluePose   = new Pose(440, 150,  90);
    public static Pose RectGreenPose  = new Pose(390, 490, 180);
    public static Pose RectYellowPose = new Pose(100,  490, 180);


    public static Pose RectDeliveryPose = new Pose(95,  38);
    public static Pose RectRecyclePose = new Pose(245,  300);
}

/*
dotnet build
dotnet run
*/
