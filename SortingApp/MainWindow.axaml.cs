using Avalonia.Controls;
using System;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;

namespace SortingApp;

public partial class MainWindow : Window
{
    ObservableCollection<newRow> rows = new ObservableCollection<newRow>();
    List<Athleten> athleten = new List<Athleten>();
    Athleten athlet;
    Laufern laufern;
    Mittelstrecke mittelstrecke;
    Werfern werfern;
    Kugelstoßern kugelstoßer;
    int index = 0;

    public MainWindow()
    {
        InitializeComponent();
        dataGrid.ItemsSource = rows;
        athlet = new Athleten("Richard", "Leichtathletik");
    }

    private void sortierenButton_Click(object sender, RoutedEventArgs e)
    {

    }

    private void hinzufuegenButton_Click(object sender, RoutedEventArgs e)
    {
        
    }

    private void auflistenButton_Click(object sender, RoutedEventArgs e)
    {

    }
}

public class newRow
{
    public string Nummer { get; set; }
    public string Name { get; set; }
    public string Disziplin { get; set; }
}