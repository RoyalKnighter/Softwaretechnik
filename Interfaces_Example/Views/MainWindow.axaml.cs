using System.Collections.Generic;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Interfaces_Example.Models;

namespace Interfaces_Example.Views;

public partial class MainWindow : Window
{
    ClassFromHannah hannah = new ClassFromHannah();
    ClassFromMarcel marcel = new ClassFromMarcel();
    ClassFromMathias mathias = new ClassFromMathias();
    ClassFromNadine nadine = new ClassFromNadine();
    ClassFromPhillip phillip = new ClassFromPhillip();

    List<IDescription> descriptions = new List<IDescription>();

    public MainWindow()
    {
        InitializeComponent();

        descriptions.Add(hannah);
        descriptions.Add(marcel);
        descriptions.Add(nadine);
        descriptions.Add(phillip);
        descriptions.Add(mathias);
    }

    private void Button1_Click(object sender, RoutedEventArgs e)
    {
        foreach (IDescription description in descriptions)
        {
            if (description is ClassFromMarcel)
            {
                TextBox1.Text = description.Run();
                return;
            }
        }
    }
    private void Button2_Click(object sender, RoutedEventArgs e)
    {
        foreach (IDescription description in descriptions)
            if (description is ClassFromPhillip)
            {
                TextBox2.Text = description.Run();
                return;
            }

    }
    private void Button3_Click(object sender, RoutedEventArgs e)
    {
        foreach (IDescription description in descriptions)
        {
            if (description is ClassFromNadine)
            {
                TextBox3.Text = description.Run();
                return;
            }
        }
    }
    private void Button4_Click(object sender, RoutedEventArgs e)
    {
        foreach (IDescription description in descriptions)
        {
            if (description is ClassFromMathias)
            {
                TextBox4.Text = description.Run();
                return;
            }
        }
    }
    private void Button5_Click(object sender, RoutedEventArgs e)
    {
        foreach (IDescription description in descriptions)
        {
            if (description is ClassFromHannah)
            {
                TextBox5.Text = description.Run();
                return;
            }
        }
    }

    private void Button6_Click(object sender, RoutedEventArgs e)
    {
        TextBox1.Text = "->";
        TextBox2.Text = "->";
        TextBox3.Text = "->";
        TextBox4.Text = "->";
        TextBox5.Text = "->";
    }
}