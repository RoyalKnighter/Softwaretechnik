using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SingletonPattern
{
    /// <summary>
    /// Interaktionslogik für MainWindow.axaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonInstanz1_Click(object sender, RoutedEventArgs e)
        {
           Singleton singleton = Singleton.CreateSingleton("Objekt 1");
           textAusgabe1.Text = singleton.sayHi();
        }

        private void buttonInstanz2_Click(object sender, RoutedEventArgs e)
        {
           Singleton singleton = Singleton.CreateSingleton("Objekt 2");
           textAusgabe2.Text = singleton.sayHi();
        }
    }
}
