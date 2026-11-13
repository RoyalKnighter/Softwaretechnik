using Avalonia.Controls;
using Avalonia.Interactivity;
using PizzeriaProgramm.Models;
using PizzeriaProgramm.Models.PizzaSorten;

namespace PizzeriaProgramm.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Köln bestellt Salami, Berlin bestellt Spinat.
        Bestelle(new KoelnPizzeria(), "Salami");
        Bestelle(new BerlinPizzeria(), "Spinat");
    }

    private void Bestellen_Click(object sender, RoutedEventArgs e)
    {
        // Die konkrete Fabrik (Unterklasse von Pizzeria) wird gewählt –
        // der Client kennt danach nur noch die abstrakte Klasse Pizzeria.
        Pizzeria pizzeria = PizzeriaAuswahl.SelectedIndex == 1
            ? new BerlinPizzeria()
            : new KoelnPizzeria();

        string typ = (PizzaAuswahl.SelectedItem as ComboBoxItem)?.Content as string ?? "Salami";
        Bestelle(pizzeria, typ);
    }

    private void Leeren_Click(object sender, RoutedEventArgs e)
    {
        Ausgabe.Text = string.Empty;
    }

    // Die Pizzeria liefert die fertige Pizza, die View zeigt ihr Protokoll an.
    private void Bestelle(Pizzeria pizzeria, string typ)
    {
        Pizza pizza = pizzeria.bestellePizza(typ);
        Ausgabe.Text += pizza.Protokoll + "\n";
        Ausgabe.CaretIndex = Ausgabe.Text.Length;
    }
}