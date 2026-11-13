using PizzeriaProgramm.Models.PizzaSorten;
using PizzeriaProgramm.Models.PizzaSorten.Koeln;

namespace PizzeriaProgramm.Models;

public class KoelnPizzeria : Pizzeria
{
    public override Pizza erstellePizza(string typ)
    {
        return typ switch
        {
            "Spinat" => new KoelnSpinatPizza(),
            "Schinken" => new KoelnSchinkenPizza(),
            "Thunfish" => new KoelnThunfushPizza(),
            "Krabben" => new KoelnKrabbenPizza(),
            _ => new KoelnSalamiPizza()
        };
    }
}