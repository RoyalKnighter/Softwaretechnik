using System;
using PizzeriaProgramm.Models.PizzaSorten;

namespace PizzeriaProgramm.Models;

public abstract class Pizzeria
{
    public Pizza bestellePizza(String typ)
    {
        Pizza pizza;
        pizza = erstellePizza(typ);
        pizza.vorbereiten();
        pizza.backen();
        pizza.schneiden();
        pizza.einpacken();
        return pizza;
    }

    public abstract Pizza erstellePizza(String typ);
}