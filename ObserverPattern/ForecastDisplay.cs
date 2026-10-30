using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{
    internal class ForecastDisplay
    {
        private float currentPressure = 29.92f;
        private float lastPressure;
        private WeatherData weatherData;

        public ForecastDisplay(WeatherData weatherData)
        {
            this.weatherData = weatherData;
        }

        public String display()
        {
            String forecast = "";
            forecast += "Forecast: ";
            if (currentPressure > lastPressure)
            {
                forecast += "Improving weather on the way!";
            }
            else if (currentPressure == lastPressure)
            {
                forecast += "More of the same";
            }
            else if (currentPressure < lastPressure)
            {
                forecast += "Watch out for cooler, rainy weather";
            }
            return forecast;
        }
        
        public void update()
        {
            lastPressure = currentPressure;
            currentPressure = weatherData.getPressure();
        }//-------------------------------------------------------------------
    }
}

