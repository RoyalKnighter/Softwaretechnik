using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia;

namespace smartFactoryApp;

public class Robot
{
    private Point location;
    Rectangle body;
    int schritt = 10;

    public Robot(Canvas canvas, Point start)
    {
        body = new Rectangle
        {
            Width = 40,
            Height = 40,
            Fill = Brushes.SteelBlue
        };

        location = start;

        canvas.Children.Add(body);
        Update();
    }

    public void Bewegen(Direction direction)
    {
        int dx = 0, dy = 0;

        switch  (direction)
        {
            case Direction.left: dx = -schritt; break;
            case Direction.right: dx = schritt; break;
            case Direction.up: dy = -schritt; break;
            case Direction.down: dy = schritt; break;
        }

        location = new Point(location.X + dx, location.Y + dy);

        Update();
    }

    private void Update()
    {
        Canvas.SetLeft(body, location.X);
        Canvas.SetTop(body, location.Y);
    }
}