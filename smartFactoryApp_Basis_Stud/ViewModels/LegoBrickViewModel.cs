using CommunityToolkit.Mvvm.ComponentModel;
using smartFactoryApp.Models;

namespace smartFactoryApp.ViewModels;

// Darstellung eines Legosteins für die View.
// Das Model (LegoBrick) kennt keine View-Benachrichtigungen; dieses ViewModel liest
// die Werte aus dem Model und meldet Änderungen per OnPropertyChanged an die View.
public partial class LegoBrickViewModel : ViewModelBase
{
    private readonly LegoBrick brick;

    // [ObservableProperty] erzeugt die Properties X und Y;
    // OnPropertyChanged wird nur ausgelöst, wenn sich der Wert tatsächlich ändert
    [ObservableProperty] private double x;
    [ObservableProperty] private double y;

    public LegoBrickViewModel(LegoBrick brick)
    {
        this.brick = brick;
        Refresh();
    }

    // Übernimmt die aktuelle Position aus dem Model
    public void Refresh()
    {
        X = brick.Location.X;
        Y = brick.Location.Y;
    }
}
