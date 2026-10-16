using System;
using System.Collections.Generic;
using Avalonia.Threading;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    Factory factory;
    // Timer für die zyklische Bewegung der Legosteine (läuft im UI-Thread)
    private readonly DispatcherTimer timer;

    public double RobotX => factory.RobotX;
    public double RobotY => factory.RobotY;
    public double ProductionLineX => ViewParameters.ProductionLinePosition.X;
    public double ProductionLineY => ViewParameters.ProductionLinePosition.Y;
    public double ProductionLineWidth => ViewParameters.ProductionLineWidth;
    public double ProductionLineHeight => ViewParameters.ProductionLineHeight;

    // Je ein LegoBrickViewModel pro Legostein; an diese Objekte bindet die View (z.B. Bricks[0].X)
    public List<LegoBrickViewModel> Bricks { get; } = new List<LegoBrickViewModel>();

    public MainViewModel()
    {
        factory = new Factory();

        foreach (var brick in factory.Bricks) Bricks.Add(new LegoBrickViewModel(brick));

        timer = new DispatcherTimer
        {
            //Equivalent zu: timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            Interval = TimeSpan.FromMilliseconds(timerInterval)
        };
        timer.Tick += Timer_Tick;
    }

    // Step-Modus: der Roboter bewegt sich pro Klick genau einen Schritt
    private void RobotBewegen(Direction direction)
    {
        factory.RobotBewegen(direction);
        NotifyRobotPositionChanged();
        NotifyLegoBricksPositionChanged();
    }

    public void MoveLeft()
    {
        RobotBewegen(Direction.left);
    }
    public void MoveRight()
    {
        RobotBewegen(Direction.right);
    }
    public void MoveUp()
    {
        RobotBewegen(Direction.up);
    }
    public void MoveDown()
    {
        RobotBewegen(Direction.down);
    }

    // Startet den Timer, die Legosteine bewegen sich auf der Produktionslinie
    public void StartTimer()
    {
        timer.Start();
    }
    // Hält den Timer an, die Legosteine bleiben an ihrer aktuellen Position stehen
    public void StopTimer()
    {
        timer.Stop();
    }
    public void Undock()
    {
        factory.Undock();
        NotifyRobotPositionChanged();
        NotifyLegoBricksPositionChanged();
    }

    // Wird bei jedem Timer-Takt (alle timerInterval ms) aufgerufen
    private void Timer_Tick(object? sender, EventArgs e)
    {
        factory.LegoBricksBewegen();
        NotifyLegoBricksPositionChanged();
    }
    private void NotifyRobotPositionChanged()
    {
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    // Übernimmt für jeden Stein X und Y aus dem Model.
    // Refresh() löst dabei OnPropertyChanged aus, sodass die Rechtecke in der View nachgezogen werden.
    private void NotifyLegoBricksPositionChanged()
    {
        foreach (var brick in Bricks)
            brick.Refresh();
    }
}
