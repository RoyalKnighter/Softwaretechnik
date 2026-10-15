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
    public List<LegoBrick> Bricks { get {return bricks;}}
   
    public Factory()
    {
        robot = new Robot(RobotPose.Location, Borders);
        CreateBricks();
    }
    private void CreateBricks()
    {    
        Pose pose1 = new Pose(BrickPose.X + 0, BrickPose.Y, BrickPose.Angle);
        Pose pose2 = new Pose(BrickPose.X + 50, BrickPose.Y, BrickPose.Angle);
        Pose pose3 = new Pose(BrickPose.X + 100, BrickPose.Y, BrickPose.Angle);
        Pose pose4 = new Pose(BrickPose.X + 150, BrickPose.Y, BrickPose.Angle);
        /*Pose pose1 = new Pose(50, 200, BrickPose.Angle);
        Pose pose2 = new Pose(100, 250, BrickPose.Angle);
        Pose pose3 = new Pose(150, 250, BrickPose.Angle);
        Pose pose4 = new Pose(200, 250, BrickPose.Angle);*/
            
        bricks.Add(new LegoBrick(pose1, LegoColor.red, LegoSize.small));
        bricks.Add(new LegoBrick(pose2, LegoColor.green, LegoSize.small));
        bricks.Add(new LegoBrick(pose3, LegoColor.blue, LegoSize.small));
        bricks.Add(new LegoBrick(pose4, LegoColor.yellow, LegoSize.small));
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
                brick.Bewegen(Parameters.Direction.right);
            else
               brick.Bewegen(Parameters.Direction.left); 
        }
    }
}