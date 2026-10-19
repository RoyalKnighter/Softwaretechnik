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
        const double w = 70;   // ungedrehte Größe des Roboters, wie im XAML
        const double h = 50;
        double s = step;
        double rad = Angle * Math.PI / 180;
        double lx = 0, ly = 0;

        double cos = Math.Cos(rad); //Ankathete
        double sin = Math.Sin(rad); //Gegenkathete

        switch (direction)
        {
            case Direction.up:        ly = -s;          break;
            case Direction.down:      ly =  s;          break;
            case Direction.left:      lx = -s;          break;
            case Direction.right:     lx =  s;          break;
            case Direction.upLeft:    lx = -s; ly = -s; break;
            case Direction.upRight:   lx =  s; ly = -s; break;
            case Direction.downLeft:  lx = -s; ly =  s; break;
            case Direction.downRight: lx =  s; ly =  s; break;
            case Direction.turnLeft:  Angle -= rotationStep; break;
            case Direction.turnRight: Angle += rotationStep; break;
            case Direction.stop:      RobotDirection = Direction.stop; break;
        }

        double dx = lx * cos - ly * sin;
        double dy = lx * sin + ly * cos;

        // Winkel immer zwischen 0 und 360 halten
        Angle = (Angle % 360 + 360) % 360;

        // Gewünschter neuer Mittelpunkt (um ihn wird gedreht)
        double mitteX = X + w / 2 + dx;
        double mitteY = Y + h / 2 + dy;

        if (!Borders.IsEmpty)
        {
            // Größe der gedrehten Form
            rad = Angle * Math.PI / 180;
            cos = Math.Abs(Math.Cos(rad));
            sin = Math.Abs(Math.Sin(rad));
            double breite = w * cos + h * sin;
            double hoehe  = w * sin + h * cos;

            // Borders gilt für die linke obere Ecke, die Wand liegt eine Robotergröße weiter
            double wandLinks  = Borders.Left;
            double wandOben   = Borders.Top;
            double wandRechts = Borders.Right  + w;
            double wandUnten  = Borders.Bottom + h;

            mitteX = Math.Clamp(mitteX, wandLinks + breite / 2, wandRechts - breite / 2);
            mitteY = Math.Clamp(mitteY, wandOben  + hoehe  / 2, wandUnten  - hoehe  / 2);
        }

        X = mitteX - w / 2;
        Y = mitteY - h / 2;
    }
}
