using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObserverPattern
{
    internal interface Observer
    {
        // update for PUSH
        //void update(float temperature, float humidity, float pressure);

        // update for PULL
        void update();
    }
}
