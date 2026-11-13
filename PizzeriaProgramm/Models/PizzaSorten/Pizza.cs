namespace PizzeriaProgramm.Models.PizzaSorten;

public abstract class Pizza
{
    private string name = "";
    private string sosse = "";
    private string teig = "";
    private string protokoll = "";

    public string Name { get { return name; } protected set { name = value; } }
    public string Sosse { get { return sosse; } protected set { sosse = value; } }
    public string Teig { get { return teig; } protected set { teig = value; } }
    public string Protokoll { get { return protokoll; } }

    public void vorbereiten()
    {
        protokoll += "Deine " + name + " mit Teig " + teig + " und Soße " + sosse + " ist in der vorbereitung\n";
    }

    public void backen()
    {
        protokoll += "Die " + name + " wird gebacken\n";
    }

    public void schneiden()
    {
        protokoll += "Der " + teig + " wird geschnitten\n";
    }

    public void einpacken()
    {
        protokoll += "Die " + name + " mit " + teig + " wird eingepackt\n";
    }
}