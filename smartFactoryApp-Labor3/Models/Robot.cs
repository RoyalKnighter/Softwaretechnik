using System;
using System.Drawing;

namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
    }
    public Robot(Point location) : base(location)
    {
    }

    // Der Roboter kann sich zusätzlich drehen. Die Kollision mit der Wand
    // berücksichtigt dabei die gedrehte Form (Bounding Box).
    public override void Bewegen(Parameters.Direction direction)
    {
        const double w = 70;   // ungedrehte Größe des Roboters, wie im XAML
        const double h = 50;
        double s = Parameters.step;
        double rad = Angle * Math.PI / 180;
        double dx = 0, dy = 0;

        switch (direction)
        {
            case Parameters.Direction.up:        dy = -s;          break;
            case Parameters.Direction.down:      dy =  s;          break;
            case Parameters.Direction.left:      dx = -s;          break;
            case Parameters.Direction.right:     dx =  s;          break;
            case Parameters.Direction.upLeft:    dx = -s; dy = -s; break;
            case Parameters.Direction.upRight:   dx =  s; dy = -s; break;
            case Parameters.Direction.downLeft:  dx = -s; dy =  s; break;
            case Parameters.Direction.downRight: dx =  s; dy =  s; break;
            case Parameters.Direction.forward:   dx =  Math.Cos(rad) * s; dy =  Math.Sin(rad) * s; break;
            case Parameters.Direction.backward:  dx = -Math.Cos(rad) * s; dy = -Math.Sin(rad) * s; break;
            case Parameters.Direction.turnLeft:  Angle -= Parameters.rotationStep; break;
            case Parameters.Direction.turnRight: Angle += Parameters.rotationStep; break;
            case Parameters.Direction.stop:      break;   // nur Position prüfen
        }

        // Winkel immer zwischen 0 und 360 halten
        Angle = (Angle % 360 + 360) % 360;

        // Gewünschter neuer Mittelpunkt (um ihn wird gedreht)
        double mitteX = X + w / 2 + dx;
        double mitteY = Y + h / 2 + dy;

        if (!Borders.IsEmpty)
        {
            // Größe der gedrehten Form
            rad = Angle * Math.PI / 180;
            double cos = Math.Abs(Math.Cos(rad));
            double sin = Math.Abs(Math.Sin(rad));
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
