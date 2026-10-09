using Avalonia.Controls;

namespace smartFactoryApp.Models;

public class Factory
{
    public Robot robot;
    public Factory()
    {
        
    }

    public void initRobot(int x, int y, Image Robot)
    {
        robot = new Robot(new Avalonia.Point(x, y), Robot);
    }
}