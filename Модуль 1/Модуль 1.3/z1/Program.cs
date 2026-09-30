using System;

class Program
{
    static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static void Main()
    {
        Console.Write("Введите числитель (неотрицательный): ");
        int numerator = int.Parse(Console.ReadLine());

        Console.Write("Введите знаменатель (положительный): ");
        int denominator = int.Parse(Console.ReadLine());

        // Проверка корректности
        if (numerator < 0 || denominator <= 0)
        {
            Console.WriteLine("Ошибка: числитель ≥ 0, знаменатель > 0!");
            Console.ReadKey();
            return;
        }

        int gcd = Gcd(numerator, denominator); // Вычисление НОД числителя и знаменателя через алгоритм Евклида
        int newNumerator = numerator / gcd; // Деление числителя на НОД 
        int newDenominator = denominator / gcd; // Деление знаменателя на НОД 

        Console.WriteLine($"\nИсходная дробь:    {numerator}/{denominator}");
        Console.WriteLine($"НОД = {gcd}");
        Console.WriteLine($"Сокращённая дробь: {newNumerator}/{newDenominator}");

        Console.ReadKey();
    }
}