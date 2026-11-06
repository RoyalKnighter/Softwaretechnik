using System;
using System.Collections.Generic;
using System.Text;

namespace IntroDesignPatterns
{
    internal class GummiEnte : Ente
    {
        public GummiEnte()
        {
            altesFliegen = new FliegtGarnicht();
            setFlugverhalten(altesFliegen);
            setQuackVerhalten(new Quietscht());
            setSchwimmVerhalten(new Schwimmt());
        }

        public override String anzeigen() => "Sieht wie eine Gummiente aus";
    }
}
