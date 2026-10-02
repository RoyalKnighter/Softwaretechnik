using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia;
using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Input.TextInput;

namespace smartFactoryApp;

public class Robot
{
    private Point location;
    Image body;
    private readonly RotateTransform turn;
    int schritt = 10;
    public readonly Point normalSize = new Point(60, 40);
    public Point currentSize = new Point(60, 40);
    public readonly Point leftTop = new Point(38, 38);
    public readonly Point rightBottom = new Point(563, 563);
    int turnStatus;
    TextBlock textBlock;

    public Robot(Canvas canvas, Point start, Image robot, TextBlock text)
    {
        textBlock = text;
        body = robot;

        turn = new RotateTransform(0);
        robot.RenderTransform = turn;

        location = start;

        Update();
    }

    public void Bewegen(Direction direction)
    {
        double dx = 0, dy = 0;

        switch  (direction)
        {
            case Direction.upLeft:
                dx = -schritt;
                dy = -schritt;
            break;

            case Direction.up:
                dy = -schritt;
            break;

            case Direction.upRight:
                dx = schritt;
                dy = -schritt;
            break;

            case Direction.right:
                dx = schritt;
            break;

            case Direction.downRight:
                dx = schritt;
                dy = schritt;
            break;

            case Direction.down:
                dy = schritt;
            break;

            case Direction.downLeft:
                dx = -schritt;
                dy = schritt;
            break;

            case Direction.left:
                dx = -schritt;
            break;
        }

        Control(dx, dy);
        Update();
    }

    public void Turn(Direction direction)
    {
        switch(direction)
        {
            case Direction.turnLeft: turnStatus--; break;
            case Direction.turnRight: turnStatus++; break;
        }

        if (turnStatus % 2 == 0)
        {
            currentSize = new Point(normalSize.X, normalSize.Y);
        textBlock.Text = currentSize.X + " " + currentSize.Y + " " + location.X  + " " + location.Y;
        } else
        {
            currentSize = new Point(normalSize.Y, normalSize.X);
        textBlock.Text = currentSize.X + " " + currentSize.Y + " " + location.X  + " " + location.Y;
        }

        turn.Angle = turnStatus * 90;
    }

    private void Control(double dx, double dy)
    {
        double left = location.X + dx;
        double top = location.Y + dy;

        if (left < leftTop.X)
        {
            left = leftTop.X;
        } else if (left + currentSize.X > rightBottom.X)
        {
            left = rightBottom.X - currentSize.X;
        }

        if (top < leftTop.Y)
        {
            top = leftTop.Y;
        } else if (top + currentSize.Y > rightBottom.Y)
        {
            top = rightBottom.Y - currentSize.Y;
        }

        location = new Point(left, top);
    }

    private void Update()
    {
        Canvas.SetLeft(body, location.X);
        Canvas.SetTop(body, location.Y);
        textBlock.Text = currentSize.X + " " + currentSize.Y + " " + location.X  + " " + location.Y;
    }
}