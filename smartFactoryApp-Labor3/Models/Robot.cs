using System;
using System.Drawing;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    private Direction robotDirection = Direction.stop;

    public Direction RobotDirection
    {
        get => robotDirection;
        set => robotDirection = value;
    }

    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
    }
    public Robot(Point location) : base(location)
    {
    }

    // Der Roboter kann sich zusätzlich drehen. Die Kollision mit der Wand
    // berücksichtigt dabei die gedrehte Form (Bounding Box).
    public override void Bewegen(Direction direction)
    {
        double lx = 0, ly = 0;

        switch (direction)
        {
            case Direction.up:        ly = -step;                      break;
            case Direction.down:      ly =  step;                      break;
            case Direction.left:      lx = -step;                      break;
            case Direction.right:     lx =  step;                      break;
            case Direction.upLeft:    lx = -step; ly = -step;          break;
            case Direction.upRight:   lx =  step; ly = -step;          break;
            case Direction.downLeft:  lx = -step; ly =  step;          break;
            case Direction.downRight: lx =  step; ly =  step;          break;
            case Direction.turnLeft:  Drehen(direction); break;
            case Direction.turnRight: Drehen(direction); break;
            case Direction.stop:      RobotDirection = Direction.stop; break;
        }

        double rad = Angle * Math.PI / 180;
        double cos = Math.Cos(rad); //Ankathete
        double sin = Math.Sin(rad); //Gegenkathete
        double dx = lx * cos - ly * sin;
        double dy = lx * sin + ly * cos;

        // Gewünschter neuer Mittelpunkt (um ihn wird gedreht)
        double mitteX = X + RobotWidth / 2 + dx;
        double mitteY = Y + RobotHeight / 2 + dy;

        if (!Borders.IsEmpty)
        {
            // Größe der gedrehten Form
            double breite = RobotWidth * cos + RobotHeight * sin;
            double hoehe  = RobotWidth * sin + RobotHeight * cos;

            // Borders gilt für die linke obere Ecke, die Wand liegt eine Robotergröße weiter
            double wandLinks  = Borders.Left;
            double wandOben   = Borders.Top;
            double wandRechts = Borders.Right  + RobotWidth;
            double wandUnten  = Borders.Bottom + RobotHeight;

            mitteX = Math.Clamp(mitteX, wandLinks + breite / 2, wandRechts - breite / 2);
            mitteY = Math.Clamp(mitteY, wandOben  + hoehe  / 2, wandUnten  - hoehe  / 2);
        }

        int x = (int)Math.Round(mitteX - RobotWidth / 2);
        int y = (int)Math.Round(mitteY - RobotHeight / 2);
        
        if (!KollidiertMitProduktionslinie(x, y))
        {
            X = x;
            Y = y;
        }
    }
}
