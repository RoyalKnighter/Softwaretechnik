using System;
using System.Drawing;
using Avalonia.Win32;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
         private Point location;
         private double angle;
        private Rectangle borders;

    public Point Location { 
        get { return location; } 
        set { location = value; } 
    }
    public double Angle {get {return angle;} set {angle = value;}}
    public Rectangle Borders { get => borders; set => borders = value; }

    public Beweger(Point location, Rectangle borders)
    {
        //this.location = location;
        //this.borders = borders;
        Location = location;
        Borders = borders;
    }
    public Beweger(Point location)
    {
        Location = location;
    }
    public Beweger(Pose pose)
    {
        Location = pose.Location;
        angle = pose.Angle;
        Borders = ViewParameters.Borders;
    }
    public virtual void Bewegen(Parameters.Direction direction)
    {
        switch (direction)
        {
            case Parameters.Direction.up:
                if (Location.Y - Parameters.step >= Borders.Top)
                    Location = new Point(Location.X, Location.Y - Parameters.step);
                break;
            case Parameters.Direction.down:
                if (Location.Y + Parameters.step <= Borders.Bottom)
                    Location = new Point(Location.X, Location.Y + Parameters.step);
                break;
            case Parameters.Direction.left:
                if (Location.X - Parameters.step >= Borders.Left)
                    Location = new Point(Location.X - Parameters.step, Location.Y);
                break;
            case Parameters.Direction.right:
                if (Location.X + Parameters.step <= Borders.Right)
                    Location = new Point(Location.X + Parameters.step, Location.Y);
                break;
            default:
                break;
        }
    }
}
}