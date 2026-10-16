using Avalonia.Controls;
using Avalonia.Interactivity;
using smartFactoryApp.ViewModels;

namespace smartFactoryApp.Views;

public partial class MainWindow : Window
{
    
    public MainWindow()
    {
        InitializeComponent();
    }
    private MainViewModel? ViewModel => DataContext as MainViewModel;
    private void MoveUp_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveUp();
    private void MoveDown_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveDown();
    private void MoveLeft_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveLeft();
    private void MoveRight_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveRight();

    private void MoveUpLeft_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveUpLeft();
    private void MoveDownLeft_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveDownLeft();
    private void MoveUpRight_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveUpRight();
    private void MoveDownRight_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveDownRight();

    private void MoveForward_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveForward();
    private void MoveBackward_Click(object sender, RoutedEventArgs e) => ViewModel?.MoveBackward();

    private void TurnLeft_Click(object sender, RoutedEventArgs e) => ViewModel?.TurnLeft();
    private void TurnRight_Click(object sender, RoutedEventArgs e) => ViewModel?.TurnRight();

    private void StartBrick_Click(object sender, RoutedEventArgs e) => ViewModel?.StartBrick();
    private void StopBrick_Click(object sender, RoutedEventArgs e) => ViewModel?.StopBrick();
}