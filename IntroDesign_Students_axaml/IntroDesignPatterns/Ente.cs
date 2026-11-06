using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroDesignPatterns
{
    internal abstract class Ente
    {
        public Flugverhalten flugverhalten;
        public QuackVerhalten quackVerhalten;
        public Schwimmverhalten schwimmverhalten;
        public Flugverhalten normalFliegen;
        public String fliegen() => flugverhalten.fliegen();
        public String quaken() => quackVerhalten.quacken();
        public String schwimmen() => schwimmverhalten.schwimmen();
        public abstract String anzeigen();
        public void setFlugverhalten(Flugverhalten flugverhalten) => this.flugverhalten = flugverhalten;
        public void setQuackVerhalten(QuackVerhalten quackVerhalten) => this.quackVerhalten = quackVerhalten;
        public void setSchwimmVerhalten(Schwimmverhalten schwimmverhalten) => this.schwimmverhalten = schwimmverhalten;
        public void anderesFliegen() => setFlugverhalten((flugverhalten is FliegtRaketenbetrieben) ? normalFliegen : new FliegtRaketenbetrieben());
    }
}
