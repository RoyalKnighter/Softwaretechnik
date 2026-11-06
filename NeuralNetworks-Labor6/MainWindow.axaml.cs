using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace NeuralNetworks_1;

public partial class MainWindow : Window
{
    int inodes = 3, hnodes = 3, onodes = 3;
    nn3S nn3SO;
    double[] inputs;
    double[] targets;
    double learningRate = 0.1; //learning rate
                               // Avalonia's DataGrid has no Items.Add(), rows are added to a bound collection instead
    ObservableCollection<nodeRow> rows = new ObservableCollection<nodeRow>();
    // example values from the lab, repeated if the network has more than 3 inputs
    static readonly double[] exampleInputs = { 0.9, 0.1, 0.8 };
    const double exampleTarget = 0.9;
    public MainWindow()
    {
        InitializeComponent();
        networkDataGrid.ItemsSource = rows;
        // Avalonia has no PreviewTextInput event; a tunneling TextInput handler does the same job
        inputTextBox.AddHandler(TextInputEvent, integerTextBox_PreviewTextInput, RoutingStrategies.Tunnel);
        hiddenTextBox.AddHandler(TextInputEvent, integerTextBox_PreviewTextInput, RoutingStrategies.Tunnel);
        outputTextBox.AddHandler(TextInputEvent, integerTextBox_PreviewTextInput, RoutingStrategies.Tunnel);
        learningRateTextBox.AddHandler(TextInputEvent, learningRateTextBox_PreviewTextInput,
        RoutingStrategies.Tunnel);
    }
    // only filters keystrokes, the values themselves are read when Create is clicked
    private void integerTextBox_PreviewTextInput(object sender, TextInputEventArgs e)
    {
        e.Handled = !(e.Text ?? "").All(char.IsDigit);
    }
    private void learningRateTextBox_PreviewTextInput(object sender, TextInputEventArgs e)
    {
        e.Handled = !(e.Text ?? "").All(c => char.IsDigit(c) || c == '.' || c == ',');
    }
    private bool tryReadNodeCount(TextBox textBox, string name, out int nodes)
    {
        if (int.TryParse(textBox.Text, out nodes) && nodes > 0)
            return true;
        statusTextBlock.Text = name + " must be a whole number greater than 0.";
        return false;
    }
    private void createButton_Click(object sender, RoutedEventArgs e)
    {
        if (!tryReadNodeCount(inputTextBox, "Input Nodes", out int newInodes) ||
        !tryReadNodeCount(hiddenTextBox, "Hidden Nodes", out int newHnodes) ||
        !tryReadNodeCount(outputTextBox, "Output Nodes", out int newOnodes))
            return;
        // accept both "0.1" and "0,1", independent of the system language
        string rateText = (learningRateTextBox.Text ?? "").Replace(',', '.');
        if (!double.TryParse(rateText, NumberStyles.Float, CultureInfo.InvariantCulture, out double newLearningRate)
        || newLearningRate <= 0)
        {
            statusTextBlock.Text = "Learning Rate must be a number greater than 0, e.g. 0.1";
            return;
        }
        inodes = newInodes;
        hnodes = newHnodes;
        onodes = newOnodes;
        learningRate = newLearningRate;
        nn3SO = new nn3S(inodes, hnodes, onodes, learningRate);
        rows.Clear();
        trainButton.IsEnabled = true;
        queryButton.IsEnabled = true;
        statusTextBlock.Text = String.Format(CultureInfo.InvariantCulture,
        "Network {0}-{1}-{2} created, learning rate {3}.", inodes, hnodes, onodes, learningRate);
    }
    private void createInputs()
    {
        inputs = new double[inodes];
        for (int i = 0; i < inodes; i++)
            inputs[i] = exampleInputs[i % exampleInputs.Length];
    }
    private void trainButton_Click(object sender, RoutedEventArgs e)
    {
        createInputs();
        targets = new double[onodes];
        for (int i = 0; i < onodes; i++)
            targets[i] = exampleTarget;
        nn3SO.train(inputs, targets);
        displayResults(true);
        statusTextBlock.Text = "Trained once.";
    }
    private void queryButton_Click(object sender, RoutedEventArgs e)
    {
        createInputs();
        nn3SO.queryNN(inputs);
        displayResults(false);
        statusTextBlock.Text = "Queried.";
    }
    private static string format(double value)
    {
        return String.Format(" {0:0.##} ", value);
    }
    private static string formatWeights(double[,] weights, int row)
    {
        string column = "";
        for (int j = 0; j < weights.GetLength(1); j++)
            column += String.Format(" {0:0.##}, ", weights[row, j]);
        return column;
    }
    // one table row per node index; layers with fewer nodes leave their cells empty.
    // targets and errors only exist after training, so a query leaves them empty
    private void displayResults(bool trained)
    {
        int rowCount = Math.Max(inodes, Math.Max(hnodes, onodes));
        for (int i = 0; i < rowCount; i++)
        {
            bool isInput = i < inodes, isHidden = i < hnodes, isOutput = i < onodes;
            nodeRow data = new nodeRow
            {
                inputValue = isInput ? inputs[i].ToString() : "",
                weightsIH = isInput ? formatWeights(nn3SO.WIH, i) : "",
                inputHidden = isHidden ? format(nn3SO.Hidden_inputs[i]) : "",
                outputHidden = isHidden ? format(nn3SO.Hidden_outputs[i]) : "",
                weightsHO = isHidden ? formatWeights(nn3SO.WHO, i) : "",
                errorHidden = isHidden && trained ? format(nn3SO.Hidden_errors[i]) : "",
                inputOutput = isOutput ? format(nn3SO.Final_inputs[i]) : "",
                outputLayer = isOutput ? format(nn3SO.Final_outputs[i]) : "",
                target = isOutput && trained ? targets[i].ToString() : "",
                errorOutput = isOutput && trained ? format(nn3SO.Output_errors[i]) : "",
            };
            rows.Add(data);
        }
    }
}

public class nodeRow
{
    public string inputValue { get; set; }
    public string weightsIH { get; set; }
    public string inputHidden { get; set; }
    public string outputHidden { get; set; }
    public string weightsHO { get; set; }
    public string errorHidden { get; set; }
    public string inputOutput { get; set; }
    public string outputLayer { get; set; }
    public string target { get; set; }
    public string errorOutput { get; set; }
}

