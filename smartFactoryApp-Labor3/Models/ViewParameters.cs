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
    public Point Location => new Point(X, Y);
}

public static class ViewParameters
{
    public static Pose RobotPose = new Pose(300, 300, 0);

    // Bereich, in dem die linke obere Ecke des (ungedrehten) Roboters liegen darf.
    // Die Wand liegt also bei Right + Roboterbreite bzw. Bottom + Roboterhöhe (≈ 559 / 567).
    public static Rectangle Borders = new Rectangle(37, 37, 452, 480);

    // Dasselbe für die 12x12 großen Legosteine
    public static Rectangle BrickBorders = new Rectangle(37, 37, 510, 518);

    // Größe des Bildes einer Produktionslinie (ungedreht)
    public const int ProductionLineWidth = 100;
    public const int ProductionLineHeight = 22;

    // Linke obere Ecke und Drehwinkel der vier Produktionslinien
    public static Pose RectRedPose    = new Pose(50,  90,  315);
    public static Pose RectBluePose   = new Pose(440, 90,  45);
    public static Pose RectGreenPose  = new Pose(440, 490, 135);
    public static Pose RectYellowPose = new Pose(50,  490, 225);
}

/*
dotnet build
dotnet run
*/
