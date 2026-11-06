using System;
using System.Collections.Generic;
using System.Text;

namespace IntroDesignPatterns
{
    internal class LockEnte : Ente
    {
        public LockEnte()
        {
            altesFliegen = new FliegtGarnicht();
            setFlugverhalten(altesFliegen);
            setQuackVerhalten(new Quackgarnicht());
            setSchwimmVerhalten(new Schwimmt());
        }

        public override string anzeigen() => "Sieht aus wie eine Lockente.";
    }
}