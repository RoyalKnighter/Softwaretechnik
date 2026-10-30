using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{
    internal class WeatherData
    {
        List<Observer> observers;
        float temperature;
        float humidity;
        float pressure;

        public WeatherData()
        {
            observers = new List<Observer>();
        }
        public void registerObserver(Observer o) {  observers.Add(o); }
        public void removeObserver(Observer o) { observers.Remove(o); }

        public void notifyObserver() {}//-------------------------------------------------------------------

        public List<Observer> Observers { get { return observers; } }

        public void setMeasurements(float temperature,  float humidity, float pressure)
        {
            this.temperature = temperature;
            this.pressure = pressure;
            this.humidity = humidity;
        }

        public float getTemperature()
        {
            return temperature;
        }

        public float getHumidity()
        {
            return humidity;
        }

        public float getPressure()
        {
            return pressure;
        }

    }
}
