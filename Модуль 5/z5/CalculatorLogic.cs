using System.Globalization;   // CultureInfo, NumberStyles

namespace z5
{
    // Результат выполнения
    public class CalcResult
    {
        public bool IsError;         // была ли ошибка
        public string ErrorMessage;  // текст ошибки
        public string Display;       // что показать на дисплее
    }

    // Логика калькулятора
    public static class CalculatorLogic
    {
        // Поля состояния
        public static double FirstNumber = 0;      // первое число
        public static string Operation = "";        // текущая операция
        public static bool IsNewInput = true;       // начинаем новое число?
        public static string Display = "0";         // текст на дисплее

        // Добавление цифры
        public static void AddDigit(string digit)
        {
            if (IsNewInput)
            {
                Display = digit;
                IsNewInput = false;
            }
            else
            {
                if (Display == "0")
                    Display = digit;
                else
                    Display += digit;
            }
        }

        // Добавление точки
        public static void AddDot()
        {
            if (IsNewInput)
            {
                Display = "0.";
                IsNewInput = false;
                return;
            }

            if (!Display.Contains("."))
                Display += ".";
        }

        // Установление операции
        public static void SetOperation(string op)
        {
            FirstNumber = ParseDisplay();
            Operation = op;
            IsNewInput = true;
        }

        // Вычисление
        public static CalcResult Calculate()
        {
            var result = new CalcResult { Display = Display };

            // Операция не задана — нечего считать
            if (string.IsNullOrEmpty(Operation))
                return result;

            double secondNumber = ParseDisplay();
            double res = 0;

            switch (Operation)
            {
                case "+": res = FirstNumber + secondNumber; break;
                case "−": res = FirstNumber - secondNumber; break;
                case "×": res = FirstNumber * secondNumber; break;

                case "÷":
                    // Защита от деления на ноль
                    if (secondNumber == 0)
                    {
                        result.IsError = true;
                        result.ErrorMessage = "Деление на ноль!";
                        Clear();
                        result.Display = Display;
                        return result;
                    }
                    res = FirstNumber / secondNumber;
                    break;
            }

            // Результат -> дисплей
            Display = res.ToString(CultureInfo.InvariantCulture);

            // Сохранение результата как первое число 
            FirstNumber = res;
            Operation = "";
            IsNewInput = true;

            result.Display = Display;
            return result;
        }

        // Очистка
        public static void Clear()
        {
            Display = "0";
            FirstNumber = 0;
            Operation = "";
            IsNewInput = true;
        }

        // Backspace
        public static void Backspace()
        {
            if (IsNewInput) return;

            if (Display.Length > 1)
                Display = Display.Substring(0, Display.Length - 1);
            else
                Display = "0";
        }

        // Парсинг дисплея
        public static double ParseDisplay()
        {
            double.TryParse(Display,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double result);

            return result;
        }
    }
}