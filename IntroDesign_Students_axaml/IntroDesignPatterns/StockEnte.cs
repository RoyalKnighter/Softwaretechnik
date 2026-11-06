using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroDesignPatterns
{
    internal class StockEnte : Ente
    {
        public StockEnte()
        {
            altesFliegen = new FliegtGarnicht();
            setFlugverhalten(altesFliegen);
            setQuackVerhalten(new Quackgarnicht());
            setSchwimmVerhalten(new Schwimmt());
        }

        public override String anzeigen() => "Sieht wie eine Stockente aus";
    }
}
