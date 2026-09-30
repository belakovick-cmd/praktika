using System;

class Program
{
    // Проверка, является ли число простым
    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        for (int i = 3; i * i <= n; i += 2)
        {
            if (n % i == 0) return false;
        }
        return true;
    }

    static void Main()
    {
        Console.Write("Введите количество простых чисел K: ");
        int k = int.Parse(Console.ReadLine());

        Console.WriteLine($"\nПервые {k} простых чисел:");

        int count = 0;      // сколько простых уже нашли
        int number = 2;     // текущее проверяемое число

        while (count < k)
        {
            if (IsPrime(number))
            {
                Console.Write($"{number,6}"); // ширина поля 6 символов
                count++;

                // Каждые 10 чисел — переход на новую строку
                if (count % 10 == 0)
                    Console.WriteLine();
            }
            number++;
        }

        // Если последняя строка не закончилась переводом строки
        if (count % 10 != 0)
            Console.WriteLine();

        Console.ReadKey();
    }
}