using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{
    internal class CurrentConditionsDisplay 
    {
        float temperature;
        float humidity;
        WeatherData weatherData;

        public CurrentConditionsDisplay(WeatherData weatherData)
        {
            this.weatherData = weatherData;
        }
       
        public String display()
        {
            return("Current Conditions: " + temperature +
                "° degrees C and " + humidity + "% humidity");
        }

        public void update()
        {
            temperature = weatherData.getTemperature();
            humidity = weatherData.getHumidity();
        }
    }
}
