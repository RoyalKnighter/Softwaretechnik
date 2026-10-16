using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using smartFactoryApp.Views;

namespace smartFactoryApp.Models;

public class Beweger
{
    protected Point location;
    protected Image body;
    protected RotateTransform turn;
    protected int angle = 0;
    protected int step = 10;
    protected int angleStep = 10;

    public Beweger(Point start, Image robot)
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
            case Direction.upLeft:      dx = -step;             dy = -step;             break;
            case Direction.up:                                  dy = -step;             break;
            case Direction.upRight:     dx =  step;             dy = -step;             break;
            case Direction.right:       dx =  step;                                     break;
            case Direction.downRight:   dx =  step;             dy =  step;             break;
            case Direction.down:                                dy =  step;             break;
            case Direction.downLeft:    dx = -step;             dy =  step;             break;
            case Direction.left:        dx = -step;                                     break;
            case Direction.forward:     dx = 2 * cos * step;    dy = 2 * sin * step;    break;
            case Direction.backward:    dx = -2 * cos * step;   dy = -2 * sin * step;   break;
        }

        Control(dx, dy);
        Update();
    }

    public void Turn(Direction direction)
    {
        switch (direction)
        {
            case Direction.turnLeft:  angle -= angleStep; break;
            case Direction.turnRight: angle += angleStep; break;
        }

        turn.Angle = angle;

        Control(0, 0);
        Update();
    }

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

        if (borderLeft < MainWindow.leftTop.X)
        {
            mitteX += MainWindow.leftTop.X - borderLeft;
        } else if (borderRight > MainWindow.rightBottom.X)
        {
            mitteX -= borderRight - MainWindow.rightBottom.X;
        }

        if (borderTop < MainWindow.leftTop.Y)
        {
            mitteY += MainWindow.leftTop.Y - borderTop;
        } else if (borderBottom > MainWindow.rightBottom.Y)
        {
            mitteY -= borderBottom - MainWindow.rightBottom.Y;
        }

        location = new Point(mitteX - w / 2, mitteY - h / 2);
    }

    private void Update()
    {
        Canvas.SetLeft(body, location.X);
        Canvas.SetTop(body, location.Y);
    }
}