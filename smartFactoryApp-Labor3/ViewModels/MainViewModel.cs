using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private Factory factory;
    private DispatcherTimer timer;

    // Roboter
    public double RobotX => factory.RobotX;
    public double RobotY => factory.RobotY;
    public double RobotAngle => factory.RobotAngle;

    // Produktionslinien (bewegen sich nicht, werden nur einmal gelesen)
    public double ProductionRedX => factory.ProductionRedX;
    public double ProductionRedY => factory.ProductionRedY;
    public double ProductionRedAngle => factory.ProductionRedAngle;

    public double ProductionBlueX => factory.ProductionBlueX;
    public double ProductionBlueY => factory.ProductionBlueY;
    public double ProductionBlueAngle => factory.ProductionBlueAngle;

    public double ProductionGreenX => factory.ProductionGreenX;
    public double ProductionGreenY => factory.ProductionGreenY;
    public double ProductionGreenAngle => factory.ProductionGreenAngle;

    public double ProductionYellowX => factory.ProductionYellowX;
    public double ProductionYellowY => factory.ProductionYellowY;
    public double ProductionYellowAngle => factory.ProductionYellowAngle;

    // Legosteine je Farbe, aufbereitet für die Anzeige
    public List<LegoBrickViewModel> RedBricks { get; }
    public List<LegoBrickViewModel> BlueBricks { get; }
    public List<LegoBrickViewModel> GreenBricks { get; }
    public List<LegoBrickViewModel> YellowBricks { get; }

    public MainViewModel()
    {
        factory = new Factory();

        RedBricks    = factory.RedBricks.Select(b => new LegoBrickViewModel(b)).ToList();
        BlueBricks   = factory.BlueBricks.Select(b => new LegoBrickViewModel(b)).ToList();
        GreenBricks  = factory.GreenBricks.Select(b => new LegoBrickViewModel(b)).ToList();
        YellowBricks = factory.YellowBricks.Select(b => new LegoBrickViewModel(b)).ToList();

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
        timer.Tick += Timer_Tick;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        factory.LegoBricksBewegen();
        NotifyLegoBricksPositionChanged();
    }

    // Timer für die Legosteine
    public void StartTimer() => timer.Start();
    public void StopTimer() => timer.Stop();

    // Roboter bewegen
    public void MoveUp()        { factory.RobotBewegen(Direction.up);        NotifyRobotPositionChanged(); }
    public void MoveDown()      { factory.RobotBewegen(Direction.down);      NotifyRobotPositionChanged(); }
    public void MoveLeft()      { factory.RobotBewegen(Direction.left);      NotifyRobotPositionChanged(); }
    public void MoveRight()     { factory.RobotBewegen(Direction.right);     NotifyRobotPositionChanged(); }
    public void MoveUpLeft()    { factory.RobotBewegen(Direction.upLeft);    NotifyRobotPositionChanged(); }
    public void MoveUpRight()   { factory.RobotBewegen(Direction.upRight);   NotifyRobotPositionChanged(); }
    public void MoveDownLeft()  { factory.RobotBewegen(Direction.downLeft);  NotifyRobotPositionChanged(); }
    public void MoveDownRight() { factory.RobotBewegen(Direction.downRight); NotifyRobotPositionChanged(); }

    // Roboter drehen
    public void MoveRotateLeft()  { factory.RobotBewegen(Direction.turnLeft);  NotifyRobotPositionChanged(); }
    public void MoveRotateRight() { factory.RobotBewegen(Direction.turnRight); NotifyRobotPositionChanged(); }

    public void ResetRotation()
    {
        factory.RobotAngle = 0;
        factory.RobotBewegen(Direction.stop);   // prüft, ob der Roboter nach dem Zurückdrehen noch im Feld ist
        NotifyRobotPositionChanged();
    }

    public void Stop()
    {
        factory.RobotBewegen(Direction.stop);
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
