using System.Drawing;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace smartFactoryApp.Views;

public partial class MainWindow : Window
{
    private Robot robot;

    public MainWindow()
    {
        InitializeComponent();

        robot = new Robot(canvasFactory, new Avalonia.Point(38, 38), Robot, text);
    }

    public void UpLeftClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.upLeft);

    public void UpClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.up);

    public void UpRightClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.upRight);

    public void RightClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.right);

    public void DownRightClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.downRight);
    public void DownClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.down);

    public void DownLeftClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.downLeft);

    public void LeftClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.left);

    

    public void TurnLeftClick(object sender, RoutedEventArgs args) => robot.Turn(Direction.turnLeft);

    public void TurnRightClick(object sender, RoutedEventArgs args) => robot.Turn(Direction.turnRight);


}