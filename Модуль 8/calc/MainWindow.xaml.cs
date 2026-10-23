using System;
using System.Windows;
using System.Windows.Controls;

namespace calc
{
    public partial class MainWindow : Window
    {
        private double firstNumber = 0;
        private string operation = "";
        private bool isNewInput = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Ввод цифр
        private void BtnDigit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string digit = btn.Content.ToString();

            if (isNewInput)
            {
                txtDisplay.Text = digit;
                isNewInput = false;
            }
            else
            {
                if (txtDisplay.Text == "0")
                    txtDisplay.Text = digit;
                else
                    txtDisplay.Text += digit;
            }
        }

        // Ввод точки/запятой
        private void BtnDot_Click(object sender, RoutedEventArgs e)
        {
            if (isNewInput)
            {
                txtDisplay.Text = "0,";
                isNewInput = false;
                return;
            }

            if (!txtDisplay.Text.Contains(",") && !txtDisplay.Text.Contains("."))
                txtDisplay.Text += ",";
        }

        // Нажатие операций (+, -, *, /)
        private void BtnOperation_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            firstNumber = ParseDisplay();
            operation = btn.Content.ToString();
            isNewInput = true;
        }

        // Нажатие равно (=)
        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(operation)) return;

            double secondNumber = ParseDisplay();
            double result = 0;

            if (operation == "+") result = firstNumber + secondNumber;
            else if (operation == "-") result = firstNumber - secondNumber;
            else if (operation == "*") result = firstNumber * secondNumber;
            else if (operation == "/")
            {
                if (secondNumber == 0)
                {
                    MessageBox.Show("Деление на ноль!", "Ошибка");
                    BtnC_Click(null, null);
                    return;
                }
                result = firstNumber / secondNumber;
            }

            // Вывод результата
            txtDisplay.Text = result.ToString();

            // Сбрасывание состояния
            firstNumber = result;
            operation = "";
            isNewInput = true;
        }

        // Очистка (C)
        private void BtnC_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text = "0";
            firstNumber = 0;
            operation = "";
            isNewInput = true;
        }

        // Backspace (←)
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (isNewInput) return;

            if (txtDisplay.Text.Length > 1)
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
            else
                txtDisplay.Text = "0";
        }

        // Универсальный парсинг текста в число
        private double ParseDisplay()
        {
            // Смена запятой на точку
            string text = txtDisplay.Text.Replace(",", ".");
            if (double.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double res))
                return res;

            // Резервный вариант для русской локали
            if (double.TryParse(txtDisplay.Text, out res))
                return res;

            return 0;
        }
    }
}