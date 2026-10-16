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

/* Property um aus dem DataContext deiner View den MainViewModel herauszuholen.
Jede Avalonia‑View hat ein DataContext. Dort steckt typischerweise das ViewModel drin.
-- as MainViewModel
Der Ausdruck versucht, den DataContext in einen MainViewModel umzuwandeln.
Wenn es klappt → du bekommst ein MainViewModel.
Wenn nicht → du bekommst null (deshalb das ? im Rückgabetyp). */
    private MainViewModel? ViewModel =>
        DataContext as MainViewModel;

    private void MoveLeft_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.MoveLeft();
    }
    private void MoveRight_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.MoveRight();
    }
    private void MoveUp_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.MoveUp();
    }
    private void MoveDown_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.MoveDown();
    }
    private void StartTimer_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.StartTimer();
    }
    private void StopTimer_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.StopTimer();
    }
    private void UndockButton_Click(object? sender, RoutedEventArgs e)
    {
        ViewModel?.Undock();
    }
}
