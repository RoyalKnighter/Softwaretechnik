using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingApp
{
    class Werfern : Athleten
    {
        public Werfern(string name) : base(name)
        {

        }
        public Werfern(string name, string disziplin) : base(name, disziplin)
        {

        }

        string WerferMethode()
        {
            return ("Ich bin ein Werfer");
        }
    }
}
