using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;

namespace NeuralNetworks_1;

public partial class MainWindow : Window
{
    // ===== Felder (wie im Klassendiagramm) =====
    int inodes = 784, hnodes = 100, onodes = 10;
    double learningRate = 0.3;
    int epoches = 1;
    nn3S? nn3SO;

    double[] inputs = Array.Empty<double>();
    double[] targets = Array.Empty<double>();

    string trainFile = "";
    string testFile = "";
    string weightFile = "";

    int trainCount = 0;     // Anzahl aller bisher trainierten Datensätze
    bool trainOK = false;   // true, sobald trainiert wurde oder Gewichte geladen sind

    // Avalonia's DataGrid has no Items.Add(), rows are added to a bound collection instead
    readonly ObservableCollection<nodeRow> rows = new ObservableCollection<nodeRow>();

    public MainWindow()
    {
        InitializeComponent();
        networkDataGrid.ItemsSource = rows;

        // Avalonia has no PreviewTextInput event; a tunneling TextInput handler does the same job
        inputTextBox.AddHandler(TextInputEvent, inputTextBox_PreviewTextInput, RoutingStrategies.Tunnel);
        hiddenTextBox.AddHandler(TextInputEvent, hiddenTextBox_PreviewTextInput, RoutingStrategies.Tunnel);
        outputTextBox.AddHandler(TextInputEvent, outputTextBox_PreviewTextInput, RoutingStrategies.Tunnel);
        learningRateTextBox.AddHandler(TextInputEvent, learningRateTextBox_PreviewTextInput, RoutingStrategies.Tunnel);
        epochsTextBox.AddHandler(TextInputEvent, epochenBox_PreviewTextInput, RoutingStrategies.Tunnel);
        performanceTextBox.AddHandler(TextInputEvent, performanceBox_PreviewTextInput, RoutingStrategies.Tunnel);

        updateButtons();
    }

    // ===== Eingabefilter: lassen nur passende Zeichen durch =====
    private static bool onlyDigits(string? text) => (text ?? "").All(char.IsDigit);

    private void inputTextBox_PreviewTextInput(object? sender, TextInputEventArgs e) => e.Handled = !onlyDigits(e.Text);
    private void hiddenTextBox_PreviewTextInput(object? sender, TextInputEventArgs e) => e.Handled = !onlyDigits(e.Text);
    private void outputTextBox_PreviewTextInput(object? sender, TextInputEventArgs e) => e.Handled = !onlyDigits(e.Text);
    private void epochenBox_PreviewTextInput(object? sender, TextInputEventArgs e) => e.Handled = !onlyDigits(e.Text);

    private void learningRateTextBox_PreviewTextInput(object? sender, TextInputEventArgs e)
    {
        e.Handled = !(e.Text ?? "").All(c => char.IsDigit(c) || c == '.' || c == ',');
    }

    // Performance wird nur vom Programm geschrieben, der Benutzer darf nichts eintippen
    private void performanceBox_PreviewTextInput(object? sender, TextInputEventArgs e) => e.Handled = true;

    // ===== Netz erstellen =====
    private bool tryReadNodeCount(TextBox textBox, string name, out int nodes)
    {
        if (int.TryParse(textBox.Text, out nodes) && nodes > 0)
            return true;
        statusTextBlock.Text = name + " must be a whole number greater than 0.";
        return false;
    }

    private void createButton_Click(object? sender, RoutedEventArgs e)
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
            statusTextBlock.Text = "Learning Rate must be a number greater than 0, e.g. 0.3";
            return;
        }

        inodes = newInodes;
        hnodes = newHnodes;
        onodes = newOnodes;
        learningRate = newLearningRate;
        nn3SO = new nn3S(inodes, hnodes, onodes, learningRate);

        trainCount = 0;
        trainOK = false;
        rows.Clear();
        performanceTextBox.Text = "0";
        recognizedTextBlock.Text = "?";
        digitImage.Source = null;

        updateButtons();
        statusTextBlock.Text = String.Format(CultureInfo.InvariantCulture,
            "Network {0}-{1}-{2} created, learning rate {3}.", inodes, hnodes, onodes, learningRate);
    }

    // ===== Dateien auswählen =====
    private async void openTrainButton_Click(object? sender, RoutedEventArgs e)
    {
        string? path = await pickFile("Trainingsdatei öffnen");
        if (path == null) return;

        trainFile = path;
        statusTextBlock.Text = "Training file: " + Path.GetFileName(path);
        updateButtons();
    }

    private async void openTestButton_Click(object? sender, RoutedEventArgs e)
    {
        string? path = await pickFile("Testdatei öffnen");
        if (path == null) return;

        testFile = path;
        statusTextBlock.Text = "Test file: " + Path.GetFileName(path);
        updateButtons();
    }

    // öffnet einen Dateidialog und gibt den Pfad zurück, oder null bei Abbruch
    private async Task<string?> pickFile(string title)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("CSV / Text") { Patterns = new[] { "*.csv", "*.txt" } },
                FilePickerFileTypes.All
            }
        });
        return files.Count == 0 ? null : files[0].TryGetLocalPath();
    }

    // ===== Training =====
    private async void trainButton_Click(object? sender, RoutedEventArgs e)
    {
        if (nn3SO == null || trainFile == "")
            return;

        if (!int.TryParse(epochsTextBox.Text, out epoches) || epoches < 1)
        {
            statusTextBlock.Text = "Epochen must be a whole number greater than 0.";
            return;
        }

        setBusy(true);
        int count = 0;          // Datensätze in diesem Trainingslauf
        bool stopped = false;   // true, wenn im Dialog "Stop" gedrückt wurde

        try
        {
            for (int epoch = 1; epoch <= epoches && !stopped; epoch++)
            {
                foreach (string line in File.ReadLines(trainFile))
                {
                    if (line.Trim() == "")
                        continue;

                    int label = readInputs(line);

                    // Target: 0.01 überall, 0.99 beim Knoten der richtigen Ziffer
                    targets = new double[onodes];
                    for (int k = 0; k < onodes; k++)
                        targets[k] = 0.01;
                    targets[label] = 0.99;

                    nn3SO.train(inputs, targets);
                    trainCount++;
                    count++;

                    if (showImagesCheckBox.IsChecked == true)
                    {
                        displayResults(true);
                        digitImage.Source = createImage(inputs);
                        recognizedTextBlock.Text = label.ToString();

                        if (!await showNextDialog($"Training – Epoche {epoch}, Datensatz {count}\nZiffer: {label}"))
                        {
                            stopped = true;
                            break;
                        }
                    }
                    else if (count % 1000 == 0)
                    {
                        // gibt der Oberfläche kurz Zeit zum Zeichnen, damit das Fenster nicht einfriert
                        statusTextBlock.Text = $"Training ... epoch {epoch}, {count} records";
                        await Task.Delay(1);
                    }
                }
            }

            displayResults(true);   // zeigt den zuletzt trainierten Datensatz
            trainOK = true;

            // Gewichte neben der Trainingsdatei speichern, damit sie später geladen werden können
            weightFile = Path.Combine(Path.GetDirectoryName(trainFile) ?? "", Path.GetFileName(trainFile) + "_" + learningRate.ToString() + "_" + "weights.csv");
            saveWeights(weightFile);

            statusTextBlock.Text = $"Trained {count} records{(stopped ? " (stopped)" : "")}, " +
                                   $"total {trainCount}. Weights saved to {Path.GetFileName(weightFile)}.";
        }
        catch (Exception ex)
        {
            statusTextBlock.Text = "Training failed: " + ex.Message;
        }
        finally
        {
            setBusy(false);
        }
    }

    // ===== Testen =====
    private async void queryButton_Click(object? sender, RoutedEventArgs e)
    {
        if (nn3SO == null || testFile == "")
            return;

        setBusy(true);
        int tested = 0, correct = 0;

        try
        {
            foreach (string line in File.ReadLines(testFile))
            {
                if (line.Trim() == "")
                    continue;

                int label = readInputs(line);
                nn3SO.queryNN(inputs);

                // erkannte Ziffer = Ausgabeknoten mit dem größten Wert
                int recognized = indexOfMax(nn3SO.Final_outputs);

                tested++;
                if (recognized == label)
                    correct++;
                performanceTextBox.Text = ((double)correct / tested).ToString("0.00", CultureInfo.InvariantCulture);
                recognizedTextBlock.Text = recognized.ToString();

                if (showImagesCheckBox.IsChecked == true)
                {
                    displayResults(false);
                    digitImage.Source = createImage(inputs);

                    string result = recognized == label ? "richtig" : "falsch";
                    if (!await showNextDialog($"Query – Datensatz {tested}\nZiffer: {label}, erkannt: {recognized} ({result})"))
                        break;
                }
                else if (tested % 1000 == 0)
                {
                    statusTextBlock.Text = $"Testing ... {tested} records";
                    await Task.Delay(1);
                }
            }

            displayResults(false);
            statusTextBlock.Text = $"Tested {tested} records, {correct} correct.";
        }
        catch (Exception ex)
        {
            statusTextBlock.Text = "Query failed: " + ex.Message;
        }
        finally
        {
            setBusy(false);
        }
    }

    // ===== Gewichte laden und speichern =====
    private async void loadWeightButton_Click(object? sender, RoutedEventArgs e)
    {
        string? path = await pickFile("Gewichtsdatei öffnen");
        if (path == null) return;

        try
        {
            loadWeights(path);
            weightFile = path;
            trainOK = true;
            updateButtons();
            statusTextBlock.Text = $"Weights loaded from {Path.GetFileName(path)} ({inodes}-{hnodes}-{onodes}).";
        }
        catch (Exception ex)
        {
            statusTextBlock.Text = "Loading weights failed: " + ex.Message;
        }
    }

    // Dateiformat: 1. Zeile "inodes,hnodes,onodes", dann eine Zeile pro Zeile von wih, dann pro Zeile von who
    private void saveWeights(string path)
    {
        if (nn3SO == null) return;

        var sb = new StringBuilder();
        sb.AppendLine($"{inodes},{hnodes},{onodes}");
        appendMatrix(sb, nn3SO.WIH);
        appendMatrix(sb, nn3SO.WHO);
        File.WriteAllText(path, sb.ToString());
    }

    private static void appendMatrix(StringBuilder sb, double[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (j > 0) sb.Append(',');
                sb.Append(matrix[i, j].ToString("R", CultureInfo.InvariantCulture));
            }
            sb.AppendLine();
        }
    }

    private void loadWeights(string path)
    {
        string[] lines = File.ReadAllLines(path).Where(l => l.Trim() != "").ToArray();
        int[] sizes = lines[0].Split(',').Select(int.Parse).ToArray();

        if (lines.Length != 1 + sizes[0] + sizes[1])
            throw new InvalidDataException("the file does not match the network size in its first line.");

        // passendes Netz anlegen, falls noch keins existiert oder die Größe nicht stimmt
        if (nn3SO == null || sizes[0] != inodes || sizes[1] != hnodes || sizes[2] != onodes)
        {
            inodes = sizes[0];
            hnodes = sizes[1];
            onodes = sizes[2];
            inputTextBox.Text = inodes.ToString();
            hiddenTextBox.Text = hnodes.ToString();
            outputTextBox.Text = onodes.ToString();
            nn3SO = new nn3S(inodes, hnodes, onodes, learningRate);
        }

        // WIH und WHO liefern die Arrays selbst (Referenz), deshalb kann man hineinschreiben
        readMatrix(lines, 1, nn3SO.WIH);
        readMatrix(lines, 1 + inodes, nn3SO.WHO);
    }

    private static void readMatrix(string[] lines, int firstLine, double[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string[] values = lines[firstLine + i].Split(',');
            for (int j = 0; j < matrix.GetLength(1); j++)
                matrix[i, j] = double.Parse(values[j], CultureInfo.InvariantCulture);
        }
    }

    // ===== Datensatz einlesen =====
    // Zeile "label,p1,p2,...,p784" -> inputs (0.01 ... 1.0), Rückgabe = label
    private int readInputs(string line)
    {
        string[] values = line.Split(',');
        if (values.Length - 1 != inodes)
            throw new InvalidDataException($"a record has {values.Length - 1} pixels, but the network has {inodes} input nodes.");

        inputs = new double[inodes];
        for (int i = 1; i < values.Length; i++)
            inputs[i - 1] = int.Parse(values[i], CultureInfo.InvariantCulture) * 0.99 / 255.0 + 0.01;

        int label = int.Parse(values[0], CultureInfo.InvariantCulture);
        if (label < 0 || label >= onodes)
            throw new InvalidDataException($"label {label} does not fit {onodes} output nodes.");
        return label;
    }

    // ===== Anzeige =====
    private static string format(double value) => String.Format(" {0:0.##} ", value);

    private static string formatWeights(double[,] weights, int row)
    {
        string column = "";
        for (int j = 0; j < weights.GetLength(1); j++)
            column += String.Format(" {0:0.##}, ", weights[row, j]);
        return column;
    }

    // one table row per node index; layers with fewer nodes leave their cells empty.
    // targets and errors only exist after training, so a query leaves them empty.
    // weights are only shown for small networks (784 x 100 do not fit into a column)
    private void displayResults(bool trained)
    {
        if (nn3SO == null) return;

        rows.Clear();
        bool showWeights = Math.Max(inodes, Math.Max(hnodes, onodes)) <= 10;
        int rowCount = Math.Max(inodes, Math.Max(hnodes, onodes));

        for (int i = 0; i < rowCount; i++)
        {
            bool isInput = i < inodes, isHidden = i < hnodes, isOutput = i < onodes;
            rows.Add(new nodeRow
            {
                inputValue   = isInput ? format(inputs[i]) : "",
                weightsIH    = isInput && showWeights ? formatWeights(nn3SO.WIH, i) : "",
                inputHidden  = isHidden ? format(nn3SO.Hidden_inputs[i]) : "",
                outputHidden = isHidden ? format(nn3SO.Hidden_outputs[i]) : "",
                weightsHO    = isHidden && showWeights ? formatWeights(nn3SO.WHO, i) : "",
                errorHidden  = isHidden && trained ? format(nn3SO.Hidden_errors[i]) : "",
                inputOutput  = isOutput ? format(nn3SO.Final_inputs[i]) : "",
                outputLayer  = isOutput ? format(nn3SO.Final_outputs[i]) : "",
                target       = isOutput && trained ? format(targets[i]) : "",
                errorOutput  = isOutput && trained ? format(nn3SO.Output_errors[i]) : "",
            });
        }
    }

    // macht aus 784 Werten (0.01 ... 1.0) ein 28x28-Graustufenbild, Ziffer weiß auf schwarz
    private static WriteableBitmap createImage(double[] values)
    {
        const int size = 28;
        var bmp = new WriteableBitmap(new PixelSize(size, size), new Vector(96, 96),
                                      PixelFormat.Bgra8888, AlphaFormat.Premul);

        byte[] pixels = new byte[size * size * 4];   // 4 Bytes pro Pixel: B, G, R, A
        for (int i = 0; i < size * size && i < values.Length; i++)
        {
            double ink = Math.Clamp((values[i] - 0.01) / 0.99, 0.0, 1.0);
            byte gray = (byte)(255 * ink);
            pixels[i * 4 + 0] = gray;
            pixels[i * 4 + 1] = gray;
            pixels[i * 4 + 2] = gray;
            pixels[i * 4 + 3] = 255;
        }

        using (var fb = bmp.Lock())
        {
            for (int y = 0; y < size; y++)
                Marshal.Copy(pixels, y * size * 4, fb.Address + y * fb.RowBytes, size * 4);
        }
        return bmp;
    }

    // Dialog mit Next/Stop; true = Next, false = Stop oder Fenster geschlossen
    private async Task<bool> showNextDialog(string message)
    {
        var dialog = new Window
        {
            Title = "Next",
            SizeToContent = SizeToContent.WidthAndHeight,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };

        var nextButton = new Button { Content = "Next", IsDefault = true, Width = 80 };
        var stopButton = new Button { Content = "Stop", IsCancel = true, Width = 80 };
        nextButton.Click += (_, _) => dialog.Close(true);
        stopButton.Click += (_, _) => dialog.Close(false);

        dialog.Content = new StackPanel
        {
            Margin = new Thickness(15),
            Spacing = 12,
            Children =
            {
                new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, MaxWidth = 260 },
                new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Spacing = 8,
                    Children = { stopButton, nextButton }
                }
            }
        };

        return await dialog.ShowDialog<bool>(this);
    }

    // ===== Hilfsmethoden =====
    private static int indexOfMax(double[] values)
    {
        int best = 0;
        for (int i = 1; i < values.Length; i++)
            if (values[i] > values[best])
                best = i;
        return best;
    }

    // Buttons nur aktivieren, wenn die Voraussetzungen erfüllt sind
    private void updateButtons()
    {
        createButton.IsEnabled = true;
        openTrainingFileButton.IsEnabled = true;
        openTestFileButton.IsEnabled = true;
        loadWeightsButton.IsEnabled = true;
        trainButton.IsEnabled = nn3SO != null && trainFile != "";
        queryButton.IsEnabled = nn3SO != null && trainOK && testFile != "";
    }

    // während Training/Test alle Buttons sperren, damit nichts doppelt gestartet wird
    private void setBusy(bool busy)
    {
        if (busy)
        {
            createButton.IsEnabled = false;
            openTrainingFileButton.IsEnabled = false;
            openTestFileButton.IsEnabled = false;
            loadWeightsButton.IsEnabled = false;
            trainButton.IsEnabled = false;
            queryButton.IsEnabled = false;
        }
        else
        {
            updateButtons();
        }
    }
}

public class nodeRow
{
    public string inputValue { get; set; } = "";
    public string weightsIH { get; set; } = "";
    public string inputHidden { get; set; } = "";
    public string outputHidden { get; set; } = "";
    public string weightsHO { get; set; } = "";
    public string errorHidden { get; set; } = "";
    public string inputOutput { get; set; } = "";
    public string outputLayer { get; set; } = "";
    public string target { get; set; } = "";
    public string errorOutput { get; set; } = "";
}
