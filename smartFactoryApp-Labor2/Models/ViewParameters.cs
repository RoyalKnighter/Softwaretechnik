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
        public Point Location => new Point(X,Y);
    }

    public static class ViewParameters
    {
        public static Pose RobotPose = new Pose(300, 300, 0);
        public static Pose RedBrickPose = new Pose(90, 130, 45);
        public static Pose BlueBrickPose = new Pose(490, 130, 45);
        public static Pose GreenBrickPose = new Pose(490, 460, 45);
        public static Pose YellowBrickPose = new Pose(90, 460, 45);
        public static Rectangle Borders = new Rectangle(37, 37, 522, 530);
    }

/*
dotnet build
dotnet run
*/