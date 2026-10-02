using CommunityToolkit.Mvvm.ComponentModel;

namespace Labor_1.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string Greeting { get; set; } = "Welcome!";
}
