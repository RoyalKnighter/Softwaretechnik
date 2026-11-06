using System;
using System.Collections.Generic;
using System.Text;

namespace IntroDesignPatterns
{
    internal class GummiEnte : Ente
    {
        public GummiEnte()
        {
            normalFliegen = new FliegtGarnicht();
            setFlugverhalten(normalFliegen);
            setQuackVerhalten(new Quietscht());
            setSchwimmVerhalten(new Schwimmt());
        }

        public override String anzeigen() => "Sieht wie eine Gummiente aus";
    }
}
