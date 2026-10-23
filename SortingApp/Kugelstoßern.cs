using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingApp
{
    class Kugelstoßern : Werfern
    {
        public Kugelstoßern(string name) : base(name)
        {

        }
        public Kugelstoßern(string name, string disziplin) : base(name, disziplin)
        {

        }

        string KuggelStosserMethode()
        {
            return ("Ich bin ein KuggelStosser");
        }
    }
}
