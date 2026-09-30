using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine();

        Console.WriteLine($"{lastName}, {firstName}");
        Console.ReadKey();
    }
}