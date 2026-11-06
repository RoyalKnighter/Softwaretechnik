namespace IntroDesignPatterns
{
    internal class ModellEnte : Ente
    {
        public ModellEnte()
        {
            normalFliegen = new FliegtGarnicht();
            setFlugverhalten(normalFliegen);
            setQuackVerhalten(new Quackgarnicht());
            setSchwimmVerhalten(new SchwimmtGarnicht());
        }

        public override string anzeigen() => "Sieht aus wie eine Modellente";
    }
}