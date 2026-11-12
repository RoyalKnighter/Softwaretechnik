using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
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

    // Im UML Felder; [ObservableProperty] erzeugt daraus die Eigenschaften RedOrder, RedDelivered usw.
    public int redOrder => factory.LegoOrder[0];
    public int blueOrder => factory.LegoOrder[1];
    public int greenOrder => factory.LegoOrder[2];
    public int yellowOrder => factory.LegoOrder[3];

    public int redDelivered => factory.LegoDelivered[0];
    public int blueDelivered => factory.LegoDelivered[1];
    public int greenDelivered => factory.LegoDelivered[2];
    public int yellowDelivered => factory.LegoDelivered[3];

    [ObservableProperty] private string orderInfo = "Test";

    // NEU (UML): Text zum Ergebnis des letzten Undock
    [ObservableProperty] private string undockInfo = "";

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

    // NEU (UML): Größe einer Produktionslinie
    public double ProductionLineWidth => ViewParameters.ProductionLineWidth;
    public double ProductionLineHeight => ViewParameters.ProductionLineHeight;

    // NEU (UML): Pfeile zum Kunden und zum Recycling
    public double ToCustomerX => ViewParameters.ToCustomerLinePosition.X;
    public double ToCustomerY => ViewParameters.ToCustomerLinePosition.Y;
    public double ToCustomerWidth => ViewParameters.ToCustomerLineWidth;
    public double ToCustomerHeight => ViewParameters.ToCustomerLineHeight;

    public double ToRecycleX => ViewParameters.ToRecycleLinePosition.X;
    public double ToRecycleY => ViewParameters.ToRecycleLinePosition.Y;
    public double ToRecycleWidth => ViewParameters.ToRecycleLineWidth;
    public double ToRecycleHeight => ViewParameters.ToRecycleLineHeight;

    // Legosteine je Farbe, aufbereitet für die Anzeige
    public List<LegoBrickViewModel> RedBricks { get; }
    public List<LegoBrickViewModel> BlueBricks { get; }
    public List<LegoBrickViewModel> GreenBricks { get; }
    public List<LegoBrickViewModel> YellowBricks { get; }

    // ABWEICHUNG vom UML: feste Plätze auf der Recycle-Linie; ein leerer Platz ist unsichtbar
    public List<LegoBrickViewModel> RecycleBricks { get; } = new List<LegoBrickViewModel>();

    public MainViewModel()
    {
        factory = new Factory();

        RedBricks = new List<LegoBrickViewModel>();
        BlueBricks = new List<LegoBrickViewModel>();
        GreenBricks = new List<LegoBrickViewModel>();
        YellowBricks = new List<LegoBrickViewModel>();

        for (int i = 0; i < bricksPerLine; i++)
        {
            RedBricks.Add(new LegoBrickViewModel(null));
            BlueBricks.Add(new LegoBrickViewModel(null));
            GreenBricks.Add(new LegoBrickViewModel(null));
            YellowBricks.Add(new LegoBrickViewModel(null));
            RecycleBricks.Add(new LegoBrickViewModel(null));
        }

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
        timer.Tick += Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        factory.LegoBricksBewegen();
        NotifyLegoBricksPositionChanged();

        if (!IsStepMode)
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
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.up);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.up;
        }
    }
    public void MoveDown()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.down);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.down;
        }
    }
    public void MoveLeft()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.left);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.left;
        }
    }
    public void MoveRight()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.right);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.right;
        }
    }
    public void MoveUpLeft()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.upLeft);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.upLeft;
        }
    }
    public void MoveUpRight()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.upRight);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.upRight;
        }
    }
    public void MoveDownLeft()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.downLeft);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.downLeft;
        }
    }
    public void MoveDownRight()
    {
        if (IsStepMode)
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
    public void MoveRotateLeft()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.turnLeft);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.turnLeft;
        }
    }
    public void MoveRotateRight()
    {
        if (IsStepMode)
        {
            factory.RobotBewegen(Direction.turnRight);
            NotifyRobotPositionChanged();
        }
        else
        {
            factory.RobotDirection = Direction.turnRight;
        }
    }

    public void Undock()
    {
        UndockInfo = factory.Undock() switch
        {
            UndockResult.notDocked => "Kein Stein gegriffen",
            UndockResult.notOnDeliveryLine => "Nicht über der Delivery-Linie",
            UndockResult.toCustomer => "Stein fährt zum Kunden",
            UndockResult.toRecycling => "Stein fährt zum Recycling",
            _ => ""
        };
        NotifyRobotPositionChanged();
        NotifyLegoBricksPositionChanged();   // abgelegten Stein auf der Linie anzeigen
    }

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

        if (Beweger.Dockobjekt != null) NotifyLegoBricksPositionChanged();
    }

    private void NotifyOrderChanged()
    {
        OnPropertyChanged(nameof(redOrder));
        OnPropertyChanged(nameof(blueOrder));
        OnPropertyChanged(nameof(greenOrder));
        OnPropertyChanged(nameof(yellowOrder));
    }

    public void NotifyOrderInfoChanged()
    {
        OrderInfo = "Hol einen Brick mit der Farbe " + 
        factory.orderBrickColor switch
        {
            LegoColor.red => "Rot",
            LegoColor.blue => "Blau",
            LegoColor.green => "Grün",
            _ => "Gelb"
        } + 
        "von der Produktion Line " +
        factory.orderFromLine switch
        {
            ProductionLineType.productionLineRed => "Rot",
            ProductionLineType.productionLineBlue => "Blau",
            ProductionLineType.productionLineGreen => "Grün",
            _ => "Gelb"
        };
    }

    private void NotifyLegoBricksPositionChanged()
    {
        // Auf der Recycle-Linie kommen Steine dazu oder fallen weg,
        // deshalb vor dem Aktualisieren den aktuellen Stein aus der Factory übernehmen
        Abgleichen(RedBricks, factory.RedBricks);
        Abgleichen(BlueBricks, factory.BlueBricks);
        Abgleichen(GreenBricks, factory.GreenBricks);
        Abgleichen(YellowBricks, factory.YellowBricks);
        Abgleichen(RecycleBricks, factory.RecycleBricks);
    }

    // Weist jedem ViewModel den Stein an derselben Stelle zu (oder null, wenn es keinen gibt)
    private static void Abgleichen(List<LegoBrickViewModel> viewModels, IReadOnlyList<LegoBrick> bricks)
    {
        for (int i = 0; i < viewModels.Count; i++)
        {
            viewModels[i].Brick = i < bricks.Count ? bricks[i] : null;
            viewModels[i].Refresh();
        }
    }

    public void Order()
    {
        factory.Order();
        NotifyOrderChanged();
        NotifyOrderInfoChanged();
    }

    public void SetUPProductionLines()
    {
        factory.SetUPProductionLines();
        NotifyLegoBricksPositionChanged();
    }

    public void GoToPickUP()
    {
        throw new NotImplementedException();
    }

    public void GoToDelivery()
    {
        factory.GoToDelivery();
        NotifyRobotPositionChanged();
    }
}
