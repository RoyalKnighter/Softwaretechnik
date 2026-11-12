using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.ViewModels;
public partial class LegoBrickViewModel : ViewModelBase
{
    // Der angezeigte Stein. Kann ausgetauscht werden (neuer Stein auf der Produktionslinie)
    // und ist null, wenn ein Platz auf der Recycle-Linie noch leer ist.
    private LegoBrick? brick;

    [ObservableProperty] private double x;
    [ObservableProperty] private double y;
    [ObservableProperty] private double angle;
    [ObservableProperty] private bool sichtbar;
    [ObservableProperty] private IBrush farbe = Brushes.Transparent;

    public LegoBrick? Brick
    {
        get => brick;
        set => brick = value;
    }

    public LegoBrickViewModel(LegoBrick? brick)
    {
        this.brick = brick;
        Refresh();
    }

    public void Refresh()
    {
        Sichtbar = brick != null;
        if (brick == null) return;

        X = brick.X;
        Y = brick.Y;
        Angle = brick.Angle;
        Farbe = brick.Color switch
        {
            LegoColor.red    => Brushes.Red,
            LegoColor.blue   => Brushes.Blue,
            LegoColor.green  => Brushes.Green,
            LegoColor.yellow => Brushes.Yellow,
            _                => Brushes.Gray
        };
    }
}
