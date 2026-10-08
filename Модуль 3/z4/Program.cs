using System;
using System.Collections.Generic;

// Делегат для фильтра
delegate bool FilterPredicate(string item);

class Program
{
    // Методы фильтры
    // Фильтр по ключевому слову
    static bool ContainsWord(string item)
    {
        Console.Write("Введите ключевое слово: ");
        string word = Console.ReadLine();
        return item.ToLower().Contains(word.ToLower());
    }

    // Фильтр по длине строки
    static bool LongerThan(string item)
    {
        Console.Write("Минимальная длина: ");
        int min = int.Parse(Console.ReadLine());
        return item.Length >= min;
    }

    // Фильтр по первой букве
    static bool StartsWith(string item)
    {
        Console.Write("Первая буква: ");
        string letter = Console.ReadLine();
        return item.StartsWith(letter, StringComparison.OrdinalIgnoreCase);
    }

    // Фильтр "содержит цифру"
    static bool HasDigit(string item)
    {
        foreach (char c in item)
            if (char.IsDigit(c)) return true;
        return false;
    }

    static void Main()
    {
        // Исходные данные
        List<string> data = new List<string>
        {
            "Отчёт за январь",
            "Письмо от Ивана",
            "Задача 42",
            "Совещание в 15:00",
            "План на февраль",
            "Позвонить клиенту",
            "Проект 2024",
            "Договор №7"
        };

        // Массив фильтров и их названий
        FilterPredicate[] filters =
        {
            ContainsWord,     // по ключевому слову
            LongerThan,       // по длине
            StartsWith,       // по первой букве
            HasDigit          // содержит цифру
        };

        string[] names =
        {
            "По ключевому слову",
            "По длине строки",
            "По первой букве",
            "Содержит цифру"
        };

        // Меню выбора фильтра
        Console.WriteLine("Данные:");
        foreach (string item in data)
            Console.WriteLine($"  {item}");

        Console.WriteLine("\nВыберите фильтр:");
        for (int i = 0; i < filters.Length; i++)
            Console.WriteLine($"  {i + 1} — {names[i]}");

        Console.Write("\nВыбор: ");
        int choice = int.Parse(Console.ReadLine()) - 1;

        // Выбор делегата
        FilterPredicate filter = filters[choice];

        // Применение фильтра к каждому элементу
        Console.WriteLine($"\n Результат: {names[choice]} \n");
        foreach (string item in data)
        {
            if (filter(item))   // вызов делегата
                Console.WriteLine($"  {item}");
        }

        Console.ReadKey();
    }
}