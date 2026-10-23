using System.Drawing;

namespace smartFactoryApp.Models
{
    public static class ViewParameters
    {
        // Startposition des Roboters (linke obere Ecke, muss innerhalb von Borders liegen!)
        public static readonly Point RobotPosition = new Point(285, 250);

        // Position der Produktionslinie (linke obere Ecke)
        public static readonly Point ProductionLinePosition = new Point(220, 100);

        // Größe der Produktionslinie in der View (Rectangle Width/Height)
        public const int ProductionLineWidth = 160;
        public const int ProductionLineHeight = 60;
        public static readonly Point DeliveryLinePosition = new Point(50, 100);

        // Größe der Produktionslinie in der View (Rectangle Width/Height)
        public const int DeliveryLineWidth = 160;
        public const int DeliveryLineHeight = 60;

        // Bewegungsbereich des Roboters (X, Y, Width, Height)
        public static readonly Rectangle Borders = new Rectangle(37, 37, 490, 460);

        // Bewegungsbereich der Legosteine: die ganze Fabrikfläche (38..563),
        // da X/Y bei den Steinen der Mittelpunkt ist (beim Roboter die linke obere Ecke)
        public static readonly Rectangle BrickBorders = new Rectangle(38, 38, 525, 525);
    }
}
