using System;
using System.Collections.Generic;

// Делегат для выполнения задачи
delegate void TaskAction(string task);

class Program
{
    // Методы, которые будут делегатами
    // Отправка уведомления
    static void SendNotification(string task)
    {
        Console.WriteLine($"  [Уведомление] Задача \"{task}\" выполнена!");
    }

    // Запись в журнал
    static void WriteToLog(string task)
    {
        Console.WriteLine($"  [Журнал] {DateTime.Now:HH:mm:ss} — задача \"{task}\"");
    }

    // Вывод на экран
    static void PrintToConsole(string task)
    {
        Console.WriteLine($"  [Консоль] >>> {task}");
    }

    static void Main()
    {
        // Список задач с делегатами
        // Каждая задача — пара
        List<(string Task, TaskAction Action)> tasks = new List<(string, TaskAction)>();

        // Добавление задач с разными делегатами
        tasks.Add(("Проверить почту", SendNotification));
        tasks.Add(("Сохранить отчёт", WriteToLog));
        tasks.Add(("Обновить базу", PrintToConsole));
        tasks.Add(("Отправить письмо", SendNotification));
        tasks.Add(("Сделать резервную копию", WriteToLog));

        // Выполнение всех задач
        Console.WriteLine("Выполнение задач\n");
        foreach (var (task, action) in tasks)
        {
            Console.WriteLine($"Задача: {task}");
            action(task);   // вызов делегата
            Console.WriteLine();
        }

        Console.ReadKey();
    }
}