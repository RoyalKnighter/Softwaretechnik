using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingApp
{
    class Athleten
    {
        string name = "";
        int age; //
        string disziplin = "";
        public int Age { get { return age; } }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Athleten(string name)
        {
            this.name = name;
            age = 20 - this.name.Length;
        }

        public Athleten(string name, string disziplin)
        {
            this.name = name;
            this.disziplin = disziplin;
            age = 20 - this.name.Length;
        }

        string AthletenMethode()
        {
            return ("Ich bin ein Athlet");
        }
    }
}
