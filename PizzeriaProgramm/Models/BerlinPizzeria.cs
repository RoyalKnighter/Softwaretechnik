using PizzeriaProgramm.Models.PizzaSorten;
using PizzeriaProgramm.Models.PizzaSorten.Berlin;

namespace PizzeriaProgramm.Models;

public class BerlinPizzeria : Pizzeria
{
    public override Pizza erstellePizza(string typ)
    {
        return typ switch
        {
            "Spinat" => new BerlinSpinatPizza(),
            "Schinken" => new BerlinSchinkenPizza(),
            "Thunfish" => new BerlinThunfishPizza(),
            "Krabben" => new BerlinKrabbenPizza(),
            _ => new BerlinSalamiPizza()
        };
    }
}