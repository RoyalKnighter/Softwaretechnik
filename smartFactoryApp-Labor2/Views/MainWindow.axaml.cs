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
    private void MoveUp_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveUp();
    }
    private void MoveDown_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveDown();
    }
    private void MoveLeft_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveLeft();
    }
    private void MoveRight_Click(object sender, RoutedEventArgs e)
    {
        ViewModel?.MoveRight();
    }
}