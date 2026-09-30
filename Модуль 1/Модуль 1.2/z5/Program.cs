using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Введите количество элементов K: ");
        int k = int.Parse(Console.ReadLine());

        // Русский алфавит (заглавные буквы)
        string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        // Гласные буквы русского алфавита
        string vowels = "АЕЁИОУЫЭЮЯ";

        Random random = new Random();
        char[] firstArray = new char[k];

        // Заполняем массив случайными буквами
        for (int i = 0; i < k; i++)
        {
            firstArray[i] = alphabet[random.Next(alphabet.Length)];
        }

        // Формируем список согласных
        List<char> consonants = new List<char>();
        foreach (char c in firstArray)
        {
            if (!vowels.Contains(c))
                consonants.Add(c);
        }

        char[] secondArray = consonants.ToArray();

        // Вывод исходного массива
        Console.WriteLine($"\nИсходный массив ({firstArray.Length} элементов):");
        PrintArray(firstArray);

        // Вывод массива согласных
        Console.WriteLine($"\nМассив согласных ({secondArray.Length} элементов):");
        PrintArray(secondArray);

        Console.ReadKey();
    }

    static void PrintArray(char[] array, int perLine = 20)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write($"{array[i]} ");
            if ((i + 1) % perLine == 0) Console.WriteLine();
        }
        if (array.Length % perLine != 0) Console.WriteLine();
    }
}