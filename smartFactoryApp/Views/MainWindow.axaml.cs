using Avalonia.Controls;
using Avalonia.Interactivity;

namespace smartFactoryApp.Views;

public partial class MainWindow : Window
{
    private Robot robot;

    public MainWindow()
    {
        InitializeComponent();

        robot = new Robot(canvasFactory, new Avalonia.Point(37, 37));
    }

    public void UpClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.up);

    public void DownClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.down);

    public void LeftClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.left);

    public void RightClick(object sender, RoutedEventArgs args) => robot.Bewegen(Direction.right);


}