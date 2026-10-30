using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace ObserverPattern;

public partial class MainWindow : Window
{
    //Displays
    CurrentConditionsDisplay conditionsDisplay;
    StatisticsDisplay statisticsDisplay;
    ForecastDisplay forecastDisplay;
    HeatIndexDisplay heatIndexDisplay;
    DispatcherTimer timer;
    WeatherData weatherData;
    TextBox[] textDisplays;
    Random rnd = new Random();

    public MainWindow()
    {
        timer = new DispatcherTimer();
        weatherData = new WeatherData();
        timer.Interval = TimeSpan.FromSeconds(1.0);
        timer.Tick += Timer_Tick;
        InitializeComponent();
        textDisplays = new TextBox[] {
            textWeather1, textWeather2, textWeather3, textWeather4};
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        int rndTemp = rnd.Next(-20, 20);
        int rndPress = rnd.Next(-20, 20);
        int rndHumi = rnd.Next(-20, 20);

        weatherData.setMeasurements(rndTemp, rndPress, rndHumi);

        update_Formular();
    }

    private void button_Click(object sender, RoutedEventArgs e) => timer.Start();

    private void buttonStop_Click(object sender, RoutedEventArgs e) => timer.Stop();

    // Avalonia kennt keine getrennten Checked/Unchecked-Events mehr,
    // sondern nur IsCheckedChanged. Daher ein Handler pro CheckBox.
    private void checkConditions_IsCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (checkConditions.IsChecked == true) weatherData.registerObserver((Observer) conditionsDisplay);
        else weatherData.removeObserver((Observer) conditionsDisplay);
    }

    private void checkStatistics_IsCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (checkStatistics.IsChecked == true) weatherData.registerObserver((Observer) statisticsDisplay);
        else weatherData.removeObserver((Observer) statisticsDisplay);
    }

    private void checkHeatIndex_IsCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (checkHeatIndex.IsChecked == true) weatherData.registerObserver((Observer) heatIndexDisplay);
        else weatherData.removeObserver((Observer) heatIndexDisplay);
    }

    private void checkForecast_IsCheckedChanged(object sender, RoutedEventArgs e)
    {
        if (checkForecast.IsChecked == true) weatherData.registerObserver((Observer) forecastDisplay);
        else weatherData.removeObserver((Observer) forecastDisplay);
    }

    public void update_Formular()
    {
        textTemperature.Text = weatherData.getTemperature().ToString();
        textHumidity.Text = weatherData.getHumidity().ToString();
        textPressure.Text = weatherData.getPressure().ToString();

        conditionsDisplay.update();
        statisticsDisplay.update();
        forecastDisplay.update();
        heatIndexDisplay.update();

        for (int i = 0; i < textDisplays.Length; i++)
        {
            if (weatherData.Observers.Count == i) break;

            textDisplays[i].Text = ((DisplayElement)weatherData.Observers[i]).display();
        }
    }
}