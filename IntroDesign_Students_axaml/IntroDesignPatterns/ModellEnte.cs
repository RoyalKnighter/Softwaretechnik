namespace IntroDesignPatterns
{
    internal class ModellEnte : Ente
    {
        public ModellEnte()
        {
            altesFliegen = new FliegtGarnicht();
            setFlugverhalten(altesFliegen);
            setQuackVerhalten(new Quackgarnicht());
            setSchwimmVerhalten(new SchwimmtGarnicht());
        }

        public override string anzeigen() => "Sieht aus wie eine Modellente";
    }
}