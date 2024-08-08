using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Data;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string buttonText = clickedButton.Content.ToString();

            if (Display.Text == "Error")
            {
                Display.Text = "";
            }

            // Wenn die aktuelle Anzeige genau "0" ist und die Eingabe eine Zahl ist (kein Komma, kein Operator), ersetze "0"
            if (Display.Text == "0" && buttonText != "," && buttonText != "+" && buttonText != "-" && buttonText != "x")
            {
                Display.Text = "";
            }

            // Wenn ein Komma gesetzt wird, wird geschaut ob es direkt nach einem Operator kommt und setzt dann eine 0 davor
            if (buttonText == ",")
            {
                if (Display.Text.Length > 0 && (Display.Text[^1] == '+' || Display.Text[^1] == '-' || Display.Text[^1] == 'x'))
                {
                    Display.Text += "0";
                }

                if (!CanInsertComma(Display.Text))
                {
                    return;
                }

                // Füge eine 0 hinter dem Komma hinzu
                Display.Text += ",0";
                return;
            }
            else if (buttonText == "+" || buttonText == "-" || buttonText == "x")
            {
                if (ContainsAnyOperator(Display.Text) && Display.Text[Display.Text.Length - 1] != ' ')
                {
                    return;
                }
            }

            // Ersetze "0" nach einem Komma durch die eingegebene Zahl
            if (Display.Text.EndsWith(",0") && char.IsDigit(buttonText, 0))
            {
                Display.Text = Display.Text.Substring(0, Display.Text.Length - 1) + buttonText;
            }
            else
            {
                Display.Text += buttonText;
            }
        }

        private bool CanInsertComma(string input)
        {
            int lastOperatorIndex = Math.Max(input.LastIndexOf('+'), input.LastIndexOf('-'));
            lastOperatorIndex = Math.Max(lastOperatorIndex, input.LastIndexOf('x'));
            string currentNumber = lastOperatorIndex == -1 ? input : input.Substring(lastOperatorIndex + 1);
            return !currentNumber.Contains(",");
        }

        private bool ContainsAnyOperator(string input)
        {
            return input.Contains("+") || input.Contains("-") || input.Contains("x");
        }

        private History historyWindow; // Referenz auf das History-Fenster

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = Display.Text.Replace('x', '*').Replace(',', '.');
                DataTable table = new DataTable();
                var result = table.Compute(expression, "");
                string resultText = result.ToString().Replace('.', ',');
                Display.Text = resultText;

                string historyEntry = $"{expression.Replace('*', 'x').Replace('.', ',')} = {resultText}";

                //if (historyWindow == null || !historyWindow.IsLoaded)
                //{
                //    historyWindow = new History();
                //    historyWindow.Show();
                //}

                //historyWindow.AddHistoryEntry(historyEntry);
            }
            catch (Exception ex)
            {
                Display.Text = "Error";
            }
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
        }

        private void OpenHistory_Click(object sender, RoutedEventArgs e)
        {
            History historyWindow = new History();
            historyWindow.Show();
        }

        private void Display_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}