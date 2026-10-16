namespace smartFactoryApp.Models
{
    public class Parameters
    {
        public enum Direction
        {
            left = 1,
            right = 2,
            up = 3,
            down = 4,
        }
        public enum LegoColor
        {
            yellow = 0,
            blue = 1,
            red = 2,
            green = 3,
        }
        public const int bricksPerLine = 4; // Anzahl Legosteine pro Produktionslinie
        public const int step = 2;          // Schrittweite der Legosteine pro Timer-Takt
        public const int robotStep = 5;     // Schrittweite des Roboters pro Klick
        public const int timerInterval = 50; // Timer-Intervall in Millisekunden
    }
}
