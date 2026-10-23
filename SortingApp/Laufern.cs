using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingApp
{
    class Laufern : Athleten
    {
        public Laufern(string name) : base(name)
        {

        }
        public Laufern(string name, string disziplin) : base(name, disziplin)
        {

        }

        public string LaufernMethode()
        {
            return ("Ich bin ein Laufer");
        }
    }
}
