using Avalonia.Controls;
using Avalonia.Interactivity;

namespace IntroDesignPatterns
{
    /// <summary>
    /// Interaktionslogik für MainWindow.axaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MoorEnte moorEnte = new MoorEnte();
        StockEnte stockEnte = new StockEnte();
        GummiEnte gummiEnte = new GummiEnte();
        LockEnte lockEnte = new LockEnte();
        ModellEnte modellEnte = new ModellEnte();

        public MainWindow() => InitializeComponent();

        private void button_Click(object? sender, RoutedEventArgs e)
        {
            textBoxStock.Text = stockEnte.anzeigen();
            textBoxMoor.Text = moorEnte.anzeigen();
            textBoxGummi.Text = gummiEnte.anzeigen();
            textBoxLock.Text = lockEnte.anzeigen();
            textBoxModell.Text = modellEnte.anzeigen();
        }
        private void button_Copy_Click(object? sender, RoutedEventArgs e)
        {
            textBoxStock.Text = stockEnte.fliegen();
            textBoxMoor.Text = moorEnte.fliegen();
            textBoxGummi.Text = gummiEnte.fliegen();
            textBoxLock.Text = lockEnte.fliegen();
            textBoxModell.Text = modellEnte.fliegen();
        }
        private void button_Quaken_Click(object? sender, RoutedEventArgs e)
        {
            textBoxStock.Text = stockEnte.quaken();
            textBoxMoor.Text = moorEnte.quaken();
            textBoxGummi.Text = gummiEnte.quaken();
            textBoxLock.Text = lockEnte.quaken();
            textBoxModell.Text = modellEnte.quaken();
        }

        private void button_Schwimmen_Click(object? sender, RoutedEventArgs e)
        {
            textBoxStock.Text = stockEnte.schwimmen();
            textBoxMoor.Text = moorEnte.schwimmen();
            textBoxGummi.Text = gummiEnte.schwimmen();
            textBoxLock.Text = lockEnte.schwimmen();
            textBoxModell.Text = modellEnte.schwimmen();
        }

        private void button_Raketen_Click(object? sender, RoutedEventArgs e)
        {
            stockEnte.anderesFliegen();
            moorEnte.anderesFliegen();
            gummiEnte.anderesFliegen();
            lockEnte.anderesFliegen();
            modellEnte.anderesFliegen();
        }
    }
}
