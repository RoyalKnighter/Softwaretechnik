using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private bool isStepMode = true;
    private Factory factory;
    private DispatcherTimer timer;

    int redOrder, blueOrder, greenOrder, yellowOrder;
    int redDelivered, blueDelivered, greenDelivered, yellowDelivered;
    string orderInfo = "";

    // Roboter
    public double RobotX => factory.RobotX;
    public double RobotY => factory.RobotY;
    public double RobotAngle => factory.RobotAngle;

    // Produktionslinien (bewegen sich nicht, werden nur einmal gelesen)
    public double ProductionRedX => ViewParameters.RectRedPose.X;
    public double ProductionRedY => ViewParameters.RectRedPose.Y;
    public double ProductionRedAngle => ViewParameters.RectRedPose.Angle;

    public double ProductionBlueX => ViewParameters.RectBluePose.X;
    public double ProductionBlueY => ViewParameters.RectBluePose.Y;
    public double ProductionBlueAngle => ViewParameters.RectBluePose.Angle;

    public double ProductionGreenX => ViewParameters.RectGreenPose.X;
    public double ProductionGreenY => ViewParameters.RectGreenPose.Y;
    public double ProductionGreenAngle => ViewParameters.RectGreenPose.Angle;

    public double ProductionYellowX => ViewParameters.RectYellowPose.X;
    public double ProductionYellowY => ViewParameters.RectYellowPose.Y;
    public double ProductionYellowAngle => ViewParameters.RectYellowPose.Angle;

    public double ProductionDeliveryX => ViewParameters.RectDeliveryPose.X;
    public double ProductionDeliveryY => ViewParameters.RectDeliveryPose.Y;
    public double ProductionDeliveryAngle => ViewParameters.RectDeliveryPose.Angle;
    public double ProductionDeliveryWidth => ViewParameters.ProductionDeliveryWidth;
    public double ProductionDeliveryHeight => ViewParameters.ProductionDeliveryHeight;

    public double ProductionRecycleX => ViewParameters.RectRecyclePose.X;
    public double ProductionRecycleY => ViewParameters.RectRecyclePose.Y;
    public double ProductionRecycleAngle => ViewParameters.RectRecyclePose.Angle;

    // Legosteine je Farbe, aufbereitet für die Anzeige
    public List<LegoBrickViewModel> RedBricks { get; }
    public List<LegoBrickViewModel> BlueBricks { get; }
    public List<LegoBrickViewModel> GreenBricks { get; }
    public List<LegoBrickViewModel> YellowBricks { get; }

    public MainViewModel()
    {
        factory = new Factory();

        RedBricks = factory.RedBricks.Select(b => new LegoBrickViewModel(b)).ToList();
        BlueBricks = factory.BlueBricks.Select(b => new LegoBrickViewModel(b)).ToList();
        GreenBricks = factory.GreenBricks.Select(b => new LegoBrickViewModel(b)).ToList();
        YellowBricks = factory.YellowBricks.Select(b => new LegoBrickViewModel(b)).ToList();

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        factory.LegoBricksBewegen();
        NotifyLegoBricksPositionChanged();

        if (!isStepMode)
        {
            factory.RobotBewegen(factory.RobotDirection);
            NotifyRobotPositionChanged();
        }
    }

    // Timer für die Legosteine
    public void StartTimer() => timer.Start();
    public void StopTimer() => timer.Stop();

    // Roboter bewegen
    public void MoveUp()
    {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.up);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.up;
        }
    }
    public void MoveDown() {
        if (isStepMode)
            {
            factory.RobotBewegen(Direction.down);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.down;
        }
    }
    public void MoveLeft() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.left);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.left;
        }
    }
    public void MoveRight() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.right);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.right;
        }
    }
    public void MoveUpLeft() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.upLeft);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.upLeft;
        }
    }
    public void MoveUpRight() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.upRight);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.upRight;
        }
    }
    public void MoveDownLeft() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.downLeft);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.downLeft;
        }
    }
    public void MoveDownRight() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.downRight);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.downRight;
        }
    }

    // Roboter drehen
    public void MoveRotateLeft() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.turnLeft);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.turnLeft;
        }
    }
    public void MoveRotateRight() {
        if (isStepMode)
        {
            factory.RobotBewegen(Direction.turnRight);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.turnRight;
        }
    }

    public void Undock() => factory.Undock();

    public void ResetRotation()
    {
        factory.RobotAngle = 0;
        factory.RobotBewegen(Direction.stop);   // prüft, ob der Roboter nach dem Zurückdrehen noch im Feld ist
        NotifyRobotPositionChanged();
    }

    public void Stop()
    {
        factory.RobotBewegen(Direction.stop);
        factory.RobotDirection = Direction.stop;
        NotifyRobotPositionChanged();
    }

    private void NotifyRobotPositionChanged()
    {
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
        OnPropertyChanged(nameof(RobotAngle));
    }

    private void NotifyLegoBricksPositionChanged()
    {
        foreach (var brick in RedBricks.Concat(BlueBricks).Concat(GreenBricks).Concat(YellowBricks))
        {
            brick.Refresh();
        }
    }
}
