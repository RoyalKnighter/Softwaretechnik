namespace smartFactoryApp.Models;
public class Parameters
{
    public enum Direction
    {
        stop,
        up,
        down,
        left,
        right,
        upLeft,
        downLeft,
        upRight,
        downRight,
        forward,
        backward,
        turnLeft,
        turnRight
    }
    public enum LegoColor
    {
        yellow,
        blue,
        red,
        green,
    }
    public enum LegoSize
    {
        large,
        small,
    }
    public const int bricksPerLine = 4;
    public const double rotationStep = 10;
    public const int timerInterval = 50; // Timer-Intervall in Millisekunden
    public const int step = 3;
}
