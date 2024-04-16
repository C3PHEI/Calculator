using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Windows;
using System.Windows.Controls;
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

            // Überprüfen und Zurücksetzen des Displays, wenn es "Error" enthält
            if (Display.Text == "Error")
            {
                Display.Text = "";
            }

            if (buttonText == ",")
            {
                if (!CanInsertComma(Display.Text))
                {
                    return; // Verhindere mehr als ein Komma in der aktuellen Zahl
                }
            }
            else if (buttonText == "+" || buttonText == "-" || buttonText == "x")
            {
                if (ContainsAnyOperator(Display.Text) && Display.Text[Display.Text.Length - 1] != ' ')
                {
                    return; // Verhindere mehr als einen Operator im gesamten Ausdruck
                }
            }

            Display.Text += buttonText;
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

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = Display.Text.Replace('x', '*').Replace(',', '.'); 
                DataTable table = new DataTable();
                var result = table.Compute(expression, "");
                Display.Text = result.ToString().Replace('.', ','); 
            }
            catch (Exception ex)
            {
                Display.Text = "Error";
            }
        }
        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "";
        }

    }
}
