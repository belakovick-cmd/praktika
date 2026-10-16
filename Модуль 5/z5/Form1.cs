using System;                       
using System.Windows.Forms;         

namespace z5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Подписка на общий обработчик
            btn0.Click += Button_Click;
            btn1.Click += Button_Click;
            btn2.Click += Button_Click;
            btn3.Click += Button_Click;
            btn4.Click += Button_Click;
            btn5.Click += Button_Click;
            btn6.Click += Button_Click;
            btn7.Click += Button_Click;
            btn8.Click += Button_Click;
            btn9.Click += Button_Click;

            btnDot.Click += Button_Click;
            btnC.Click += Button_Click;
            btnBack.Click += Button_Click;

            btnAdd.Click += Button_Click;
            btnSub.Click += Button_Click;
            btnMul.Click += Button_Click;
            btnDiv.Click += Button_Click;
            btnEquals.Click += Button_Click;

            // Горячие клавиши
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            // Начальное состояние
            RefreshDisplay();
        }

        // Обновление дисплея
        private void RefreshDisplay()
        {
            txtDisplay.Text = CalculatorLogic.Display;
        }

        // Общий обработчик кнопок
        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string text = btn.Text;

            switch (text)
            {
                // Цифры
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    // Вызов метода из другого файла
                    CalculatorLogic.AddDigit(text);
                    break;

                // Точка
                case ".":
                    // Вызов метода из другого файла
                    CalculatorLogic.AddDot();
                    break;

                // Операции
                case "+":
                case "−":
                case "×":
                case "÷":
                    // Вызов метода из другого файла
                    CalculatorLogic.SetOperation(text);
                    break;

                // Равно
                case "=":
                    // Вызов метода из другого файла
                    var result = CalculatorLogic.Calculate();

                    // Если ошибка (деление на ноль)
                    if (result.IsError)
                    {
                        MessageBox.Show(result.ErrorMessage, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                // Очистка
                case "C":
                    // Вызов метода из другого файла
                    CalculatorLogic.Clear();
                    break;

                // Backspace
                case "←":
                    // Вызов метода из другого файла
                    CalculatorLogic.Backspace();
                    break;
            }

            // Обновление дисплея после любой операции
            RefreshDisplay();
        }

        // Горячие клавиши
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Цифры верхнего ряда
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                int digit = e.KeyCode - Keys.D0;
                CalculatorLogic.AddDigit(digit.ToString());
                RefreshDisplay();
                return;
            }

            // NumPad
            if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                int digit = e.KeyCode - Keys.NumPad0;
                CalculatorLogic.AddDigit(digit.ToString());
                RefreshDisplay();
                return;
            }

            // Операции и управление
            switch (e.KeyCode)
            {
                case Keys.Add:
                    CalculatorLogic.SetOperation("+");
                    break;

                case Keys.Subtract:
                    CalculatorLogic.SetOperation("−");
                    break;

                case Keys.Multiply:
                    CalculatorLogic.SetOperation("×");
                    break;

                case Keys.Divide:
                    CalculatorLogic.SetOperation("÷");
                    break;

                case Keys.Enter:
                    var result = CalculatorLogic.Calculate();
                    if (result.IsError)
                        MessageBox.Show(result.ErrorMessage, "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case Keys.Back:
                    CalculatorLogic.Backspace();
                    break;

                case Keys.Escape:
                    CalculatorLogic.Clear();
                    break;

                case Keys.Decimal:
                case Keys.OemPeriod:
                    CalculatorLogic.AddDot();
                    break;
            }

            RefreshDisplay();
        }
    }
}