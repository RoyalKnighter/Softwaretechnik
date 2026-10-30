using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{
    internal class StatisticsDisplay : Observer, DisplayElement
    {
        private float maxTemp = float.MinValue;
        private float minTemp = float.MaxValue;
        private float tempSum = 0.0f;
        private int numReadings;
        private WeatherData weatherData;

        public StatisticsDisplay(WeatherData weatherData)
        {
            this.weatherData = weatherData;
            numReadings = 0;
        }
        public void update()
        {
            float t = weatherData.getTemperature();

            if (t < minTemp) minTemp = t;
            else if (t > maxTemp) maxTemp = t;

            tempSum += t;
            numReadings++;
        }//-------------------------------------------------------------------

        public String display()
        {
            return ("Avg/Max/Min temperature = " + (tempSum / numReadings)
                + "/" + maxTemp + "/" + minTemp);
        }
    }
}

