using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroDesignPatterns
{
    internal class MoorEnte : Ente
    {
        public MoorEnte()
        {
            altesFliegen = new FliegtMitFluegeln();
            setFlugverhalten(altesFliegen);
            setQuackVerhalten(new QuacktLaut());
            setSchwimmVerhalten(new Schwimmt());
        }

        public override String anzeigen() => "Sieht wie eine Moorente aus";
    }
}
