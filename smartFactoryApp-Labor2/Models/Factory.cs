using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;
using System.Collections.Generic;

namespace smartFactoryApp.ViewModels;

public class Factory
{
    Robot robot;
    List <LegoBrick> bricks = new List<LegoBrick>();

    public double RobotX => robot.Location.X;
    public double RobotY => robot.Location.Y;
    public double RobotAngle => robot.Angle;
    public List<LegoBrick> Bricks { get {return bricks;}}
   
    public Factory()
    {
        robot = new Robot(RobotPose.Location, Borders);
        CreateBricks();
    }
    private void CreateBricks()
    {    
        Pose pose1 = new Pose(RedBrickPose.X + 15, RedBrickPose.Y - 15, RedBrickPose.Angle);
        Pose pose2 = new Pose(RedBrickPose.X - 15, RedBrickPose.Y + 15, RedBrickPose.Angle);
        Pose pose3 = new Pose(RedBrickPose.X - 30, RedBrickPose.Y + 30, RedBrickPose.Angle);
        Pose pose4 = new Pose(RedBrickPose.X + 0, RedBrickPose.Y, RedBrickPose.Angle);

        Pose pose5 = new Pose(BlueBrickPose.X - 15, BlueBrickPose.Y - 15, BlueBrickPose.Angle);
        Pose pose6 = new Pose(BlueBrickPose.X - 30, BlueBrickPose.Y - 30, BlueBrickPose.Angle);
        Pose pose7 = new Pose(BlueBrickPose.X + 15, BlueBrickPose.Y + 15, BlueBrickPose.Angle);
        Pose pose8 = new Pose(BlueBrickPose.X + 0, BlueBrickPose.Y, BlueBrickPose.Angle);

        Pose pose9 = new Pose(GreenBrickPose.X + 15, GreenBrickPose.Y - 15, GreenBrickPose.Angle);
        Pose pose10 = new Pose(GreenBrickPose.X + 30, GreenBrickPose.Y - 30, GreenBrickPose.Angle);
        Pose pose11 = new Pose(GreenBrickPose.X - 15, GreenBrickPose.Y + 15, GreenBrickPose.Angle);
        Pose pose12 = new Pose(GreenBrickPose.X + 0, GreenBrickPose.Y, GreenBrickPose.Angle);

        Pose pose13 = new Pose(YellowBrickPose.X - 15, YellowBrickPose.Y - 15, YellowBrickPose.Angle);
        Pose pose14 = new Pose(YellowBrickPose.X + 15, YellowBrickPose.Y + 15, YellowBrickPose.Angle);
        Pose pose15 = new Pose(YellowBrickPose.X + 30, YellowBrickPose.Y + 30, YellowBrickPose.Angle);
        Pose pose16 = new Pose(YellowBrickPose.X + 0, YellowBrickPose.Y, YellowBrickPose.Angle);
            
        bricks.Add(new LegoBrick(pose1, LegoColor.red, LegoSize.small));
        bricks.Add(new LegoBrick(pose2, LegoColor.red, LegoSize.small));
        bricks.Add(new LegoBrick(pose3, LegoColor.red, LegoSize.small));
        bricks.Add(new LegoBrick(pose4, LegoColor.red, LegoSize.small));

        bricks.Add(new LegoBrick(pose5, LegoColor.blue, LegoSize.small));
        bricks.Add(new LegoBrick(pose6, LegoColor.blue, LegoSize.small));
        bricks.Add(new LegoBrick(pose7, LegoColor.blue, LegoSize.small));
        bricks.Add(new LegoBrick(pose8, LegoColor.blue, LegoSize.small));

        bricks.Add(new LegoBrick(pose9, LegoColor.green, LegoSize.small));
        bricks.Add(new LegoBrick(pose10, LegoColor.green, LegoSize.small));
        bricks.Add(new LegoBrick(pose11, LegoColor.green, LegoSize.small));
        bricks.Add(new LegoBrick(pose12, LegoColor.green, LegoSize.small));

        bricks.Add(new LegoBrick(pose13, LegoColor.yellow, LegoSize.small));
        bricks.Add(new LegoBrick(pose14, LegoColor.yellow, LegoSize.small));
        bricks.Add(new LegoBrick(pose15, LegoColor.yellow, LegoSize.small));
        bricks.Add(new LegoBrick(pose16, LegoColor.yellow, LegoSize.small));
    }
    public void RobotBewegen(Direction dir)
    {
        robot.Bewegen(dir);
    }
    int tickCount = 0;
    public void LegoBricksBewegen()
    {
        bool outward = tickCount < 10;
        tickCount = (tickCount + 1) % 20;
        foreach (LegoBrick brick in Bricks)
        {   
            if (outward)
                switch (brick.Color)
                {
                    case LegoColor.red:
                        brick.Bewegen(Parameters.Direction.upRight);
                    break;
                    case LegoColor.blue:
                        brick.Bewegen(Parameters.Direction.upLeft);
                    break;
                    case LegoColor.green:
                        brick.Bewegen(Parameters.Direction.downLeft);
                    break;
                    case LegoColor.yellow:
                        brick.Bewegen(Parameters.Direction.downRight);
                    break;
                }
            else
                switch (brick.Color)
                {
                    case LegoColor.red:
                        brick.Bewegen(Parameters.Direction.downLeft);
                    break;
                    case LegoColor.blue:
                        brick.Bewegen(Parameters.Direction.downRight);
                    break;
                    case LegoColor.green:
                        brick.Bewegen(Parameters.Direction.upRight);
                    break;
                    case LegoColor.yellow:
                        brick.Bewegen(Parameters.Direction.upLeft);
                    break;
                }
        }
    }
}