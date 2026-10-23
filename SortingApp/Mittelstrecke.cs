using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingApp
{
    class Mittelstrecke : Laufern
    {
        public Mittelstrecke(string name) : base(name)
        {

        }
        public Mittelstrecke(string name, string disziplin) : base(name, disziplin)
        {

        }

        string MittelStreckerMethode()
        {
            return ("Ich bin ein MittelStrecker");
        }
    }
}
