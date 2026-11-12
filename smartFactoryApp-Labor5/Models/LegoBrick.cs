using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models
{
    public class LegoBrick : Beweger
    {
        // NEU (UML): Zustand des Steins, ersetzt die alten Flags gedockt, ausgeliefert, ZumKunden und AmZiel
        private BrickStatus status = BrickStatus.onLine;

        // NEU (UML): Platz auf der Recycle-Linie (-1 = keiner).
        // Bei einem Stein, der zum Recycling fährt, ist das der für ihn reservierte Platz.
        // Bei einem Stein auf der Recycle-Linie ist es sein Platz dort.
        private int recyclePlatz = -1;

        // NEU (UML): Position und Drehung zu recyclePlatz
        private Pose recyclePose;

        // NEU (UML): Zielpunkte (linke obere Ecke des Steins) in der Mitte der beiden Pfeile
        private int toCustomerMitteX;
        private int toCustomerMitteY;
        private int toRecycleMitteX;
        private int toRecycleMitteY;

        public LegoColor Color { get; set; }
        public LegoSize Size { get; set; }

        // NEU (UML)
        public BrickStatus Status { get => status; set => status = value; }

        // NEU (UML)
        public int RecyclePlatz { get => recyclePlatz; set => recyclePlatz = value; }

        public LegoBrick(Pose pose, LegoColor color, LegoSize size) : base(pose, ViewParameters.BrickBorders)
        {
            Color = color;
            Size = size;
            recyclePose = pose;

            toCustomerMitteX = ViewParameters.ToCustomerLinePosition.X + ViewParameters.ToCustomerLineWidth / 2 - 6;
            toCustomerMitteY = ViewParameters.ToCustomerLinePosition.Y + ViewParameters.ToCustomerLineHeight / 2 - 6;
            toRecycleMitteX = ViewParameters.ToRecycleLinePosition.X + ViewParameters.ToRecycleLineWidth / 2 - 6;
            toRecycleMitteY = ViewParameters.ToRecycleLinePosition.Y + ViewParameters.ToRecycleLineHeight / 2 - 6;
        }

        public override string ToString()
        {
            return $"{Color} {Size} LegoBrick bei ({Location.X}, {Location.Y}), {Angle}°, {Status}";
        }

        // NEU (UML): Der Roboter hat den Stein gegriffen
        public void Andocken()
        {
            Status = BrickStatus.docked;
        }

        // NEU (UML): Der Stein soll zum Kunden fahren
        public void ZumKunden()
        {
            Status = BrickStatus.toCustomer;
        }

        // NEU (UML): Der Stein soll zum Recycling fahren und dort auf den Platz "platz" kommen
        public void ZumRecycling(int platz, Pose platzPose)
        {
            Status = BrickStatus.toRecycling;
            recyclePlatz = platz;
            recyclePose = platzPose;
        }

        // Wird bei jedem Timer-Takt von der Produktionslinie aufgerufen
        public override void Bewegen(Direction direction)
        {
            // Die Heimat (pose) immer einen Schritt mit der Linie mitbewegen, auch wenn der Stein
            // gerade woanders ist. So passt er beim Zurücksetzen wieder in die Reihe.
            int altX = X, altY = Y;
            double altWinkel = Angle;
            SetzePose(pose);
            base.Bewegen(direction);
            pose = Pose;
            SetzePose(new Pose(altX, altY, altWinkel));

            switch (Status)
            {
                case BrickStatus.onLine:
                    // Stein liegt auf seiner Linie (bzw. wurde gerade zurückgesetzt)
                    SetzePose(pose);
                    break;

                case BrickStatus.toCustomer:
                    // Auf der Delivery-Linie nach rechts bis zur Mitte des Kunden-Pfeils
                    if (X < toCustomerMitteX) base.Bewegen(Direction.right);
                    else Status = BrickStatus.atCustomer;
                    break;

                case BrickStatus.toRecycling:
                    // Auf der Delivery-Linie nach links bis zur Mitte des Recycle-Pfeils
                    if (X > toRecycleMitteX) base.Bewegen(Direction.left);
                    else Status = BrickStatus.atRecycling;
                    break;
                case BrickStatus.atRecycleLine:
                    SetzePose(pose);
                    break;

                    // docked: wird nur vom Roboter bewegt
                    // atCustomer, atRecycling: wartet, bis die Factory den Stein abholt
            }
        }
    }
}
