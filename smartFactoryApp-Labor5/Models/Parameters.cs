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

    // NEU (UML): Zustand eines Legosteins
    public enum BrickStatus
    {
        onLine,        // liegt auf seiner Produktionslinie (bzw. auf der Recycle-Linie)
        docked,        // wird vom Roboter gehalten
        toCustomer,    // fährt auf der Delivery-Linie zum Kunden
        toRecycling,   // fährt auf der Delivery-Linie zum Recycling
        atCustomer,    // ist beim Kunden angekommen und wartet auf die Factory
        atRecycling    // ist beim Recycling angekommen und wartet auf die Factory
    }

    // NEU (UML): Ergebnis von Factory.Undock()
    public enum UndockResult
    {
        notDocked,           // Roboter hält keinen Stein
        notOnDeliveryLine,   // Stein ist nicht über der Delivery-Linie
        toCustomer,          // Stein fährt zum Kunden
        toRecycling          // Stein fährt zum Recycling
    }

    public const int bricksPerLine = 4;   // gilt auch für die Plätze auf der Recycle-Linie
    public const double rotationStep = 10;
    public const int timerInterval = 50; // Timer-Intervall in Millisekunden
    public const int step = 3;
}
