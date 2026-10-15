using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using Avalonia.Controls.Converters;
using System.Collections.Generic;
using Avalonia.Threading;
using System;

namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    Factory factory;
    DispatcherTimer timer;
    public double RobotX => factory.RobotX;
    public double RobotY => factory.RobotY;
    public double RobotAngle => factory.RobotAngle;

    public bool brickRun = false;
    
    public List<LegoBrickViewModel> BricksView { get; } = new List<LegoBrickViewModel>();
    public MainViewModel()
    {
        factory = new Factory();
        for (int i = 0; i < factory.Bricks.Count; i++)
        {

            BricksView.Add(new LegoBrickViewModel(factory.Bricks[i])); 
        }
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(timerInteval);
        timer.Tick += Timer_Tick; 
        timer.Start();
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (brickRun)
        {
            factory.LegoBricksBewegen();
            NotifyLegoBricksPoseChanged();
        }
    }
    public void MoveUp()
    {
        factory.RobotBewegen(Direction.up);
        NotifyRobotPoseChanged();
    }
    public void MoveDown()
    {
        factory.RobotBewegen(Direction.down);
        NotifyRobotPoseChanged();
    }
    public void MoveLeft()
    {
        factory.RobotBewegen(Direction.left);
        NotifyRobotPoseChanged();
    }
    public void MoveRight()
    {
        factory.RobotBewegen(Direction.right);
        NotifyRobotPoseChanged();
    }
    public void MoveUpLeft()
    {
        factory.RobotBewegen(Direction.upLeft);
        NotifyRobotPoseChanged();
    }
    public void MoveDownLeft()
    {
        factory.RobotBewegen(Direction.downLeft);
        NotifyRobotPoseChanged();
    }
    public void MoveUpRight()
    {
        factory.RobotBewegen(Direction.upRight);
        NotifyRobotPoseChanged();
    }
    public void MoveDownRight()
    {
        factory.RobotBewegen(Direction.downRight);
        NotifyRobotPoseChanged();
    }
    public void MoveForward()
    {
        factory.RobotBewegen(Direction.forward);
        NotifyRobotPoseChanged();
    }
    public void MoveBackward()
    {
        factory.RobotBewegen(Direction.backward);
        NotifyRobotPoseChanged();
    }
    public void TurnLeft()
    {
        factory.RobotBewegen(Direction.turnLeft);
        NotifyRobotPoseChanged();
    }
    public void TurnRight()
    {
        factory.RobotBewegen(Direction.turnRight);
        NotifyRobotPoseChanged();
    }

    public void StartBrick() => brickRun = true;
    public void StopBrick() => brickRun = false;

    private void NotifyRobotPoseChanged()
    {
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
        OnPropertyChanged(nameof(RobotAngle));
    }
    private void NotifyLegoBricksPoseChanged()
    {
        foreach (var brick in BricksView)
        {
            brick.Refresh();
        }
    }

}
