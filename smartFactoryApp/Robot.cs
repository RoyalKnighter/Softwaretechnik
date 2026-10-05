using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace smartFactoryApp;

public class Robot
{
    private Point location;
    private readonly Image body;
    private readonly RotateTransform turn;
    private readonly int schritt = 10;
    public readonly Point normalSize = new Point(60, 40);
    public readonly Point leftTop = new Point(38, 38);
    public readonly Point rightBottom = new Point(563, 563);
    private int turnStatus = 0;

    public Robot(Canvas canvas, Point start, Image robot)
    {
        body = robot;

        turn = new RotateTransform(0);
        body.RenderTransform = turn;

        location = start;

        Control(0, 0);   // Startposition gleich auf Gültigkeit prüfen
        Update();
    }

    public void Bewegen(Direction direction)
    {
        double dx = 0, dy = 0;

        double rad = turn.Angle * Math.PI / 180;
        double cos = Math.Cos(rad);
        double sin = Math.Sin(rad);

        switch (direction)
        {
            case Direction.upLeft:    dx = -schritt;          dy = -schritt;          break;
            case Direction.up:                                dy = -schritt;          break;
            case Direction.upRight:   dx =  schritt;          dy = -schritt;          break;
            case Direction.right:     dx =  schritt;                                  break;
            case Direction.downRight: dx =  schritt;          dy =  schritt;          break;
            case Direction.down:                              dy =  schritt;          break;
            case Direction.downLeft:  dx = -schritt;          dy =  schritt;          break;
            case Direction.left:      dx = -schritt;                                  break;
            case Direction.forward:   dx = 2 * cos * schritt; dy = 2 * sin * schritt; break;
            case Direction.backward:   dx = -2 * cos * schritt; dy = -2 * sin * schritt; break;
        }

        Control(dx, dy);
        Update();
    }

    public void Turn(Direction direction)
    {
        switch (direction)
        {
            case Direction.turnLeft:  turnStatus--; break;
            case Direction.turnRight: turnStatus++; break;
        }

        turnStatus = (turnStatus + 72) % 72;
        turn.Angle = turnStatus * 5;

        Control(0, 0);
        Update();
    }

    // Breite und Höhe der gedrehten Form (Bounding Box)
    private (double breite, double hoehe) GedrehteGroesse()
    {
        double rad = turn.Angle * Math.PI / 180;
        double cos = Math.Abs(Math.Cos(rad));
        double sin = Math.Abs(Math.Sin(rad));

        double w = body.Width;
        double h = body.Height;

        return (w * cos + h * sin, w * sin + h * cos);
    }

    private void Control(double dx, double dy)
    {
        var (breite, hoehe) = GedrehteGroesse();
        double w = body.Width;
        double h = body.Height;

        // Gewünschter neuer Mittelpunkt
        double mitteX = location.X + w / 2 + dx;
        double mitteY = location.Y + h / 2 + dy;

        double borderLeft = (mitteX - breite / 2);
        double borderRight = (mitteX + breite / 2);
        double borderTop = (mitteY - hoehe / 2);
        double borderBottom = (mitteY + hoehe / 2);

        if (borderLeft < leftTop.X)
        {
            mitteX += leftTop.X - borderLeft;
        } else if (borderRight > rightBottom.X)
        {
            mitteX -= borderRight - rightBottom.X;
        }

        if (borderTop < leftTop.Y)
        {
            mitteY += leftTop.Y - borderTop;
        } else if (borderBottom > rightBottom.Y)
        {
            mitteY -= borderBottom - rightBottom.Y;
        }

        location = new Point(mitteX - w / 2, mitteY - h / 2);
    }

    private void Update()
    {
        Canvas.SetLeft(body, location.X);
        Canvas.SetTop(body, location.Y);
    }
}