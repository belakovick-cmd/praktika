using System;
using System.Collections.Generic; 
using System.IO;                  
using System.Linq;                

namespace StudentManager
{
    // Класс студента
    public class Student
    {
        public string Name { get; set; }          // Имя студента
        public int Age { get; set; }              // Возраст
        public double AverageGrade { get; set; }  // Средний балл

        // Переопределение стандартного метода
        public override string ToString() => $"{Name} | Возраст: {Age} | Средний балл: {AverageGrade:F2}";
    }

    class Program
    {
        // Глобальный список, в котором хранятся все студенты во время работы программы
        static List<Student> students = new List<Student>();

        // Имя файла, в который будут сохраняться данные
        const string FileName = "students.txt";

        static void Main()
        {
            // При запуске программы сразу загрузка данных из файла в список
            LoadFromFile();

            // Бесконечный цикл для показа меню
            while (true)
            {
                Console.Clear(); // Очистка консоли перед каждым показом меню
                Console.WriteLine("УПРАВЛЕНИЕ СТУДЕНТАМИ");
                Console.WriteLine("1. Показать всех");
                Console.WriteLine("2. Добавить");
                Console.WriteLine("3. Редактировать");
                Console.WriteLine("4. Удалить");
                Console.WriteLine("5. Поиск по имени");
                Console.WriteLine("6. Сортировка по среднему баллу");
                Console.WriteLine("0. Выход (с сохранением)");
                Console.Write("\nВыбор: ");

                // Считывание того что ввёл пользователь
                string choice = Console.ReadLine();

                // В зависимости от выбора вызов нужного метода
                switch (choice)
                {
                    case "1": ShowAll(); break;
                    case "2": AddStudent(); break;
                    case "3": EditStudent(); break;
                    case "4": DeleteStudent(); break;
                    case "5": SearchStudent(); break;
                    case "6": SortStudents(); break;
                    case "0":
                        SaveToFile(); // Перед выходом сохранение все в файл
                        return;       // Выход из программы
                }
            }
        }

        // Вспомогательный метод для ввода среднего балла
        static double GetDoubleInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine().Replace('.', ','); // Автоматическая смена точки на запятую

                // Проверка: получилось ли преобразовать строку в число?
                if (double.TryParse(input, out double result))
                    return result; // Если да — возвращение результата и выход из цикла

                // Если нет — вывод ошибки и цикл повторяется
                Console.WriteLine("Ошибка! Введите корректное число (например, 4,5 или 4.5).");
            }
        }

        // Вспомогательный метод для ввода возраста
        static int GetIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result; // Успешное преобразование — возвращение

                Console.WriteLine("Ошибка! Введите целое число."); // Если буквы — повторение
            }
        }

        // Методы для работы со списком

        // 1. Показать всех
        static void ShowAll()
        {
            Console.WriteLine("\nСписок студентов:");
            if (students.Count == 0) Console.WriteLine("Пусто."); // Если список пуст

            // Проход по списку и вывод каждого студента с его номером
            for (int i = 0; i < students.Count; i++)
                Console.WriteLine($"{i + 1}. {students[i]}");

            Console.ReadKey(); // Ожидание нажатия клавиши, чтобы пользователь успел прочитать
        }

        // 2. Добавить
        static void AddStudent()
        {
            Console.Write("Имя: ");
            string name = Console.ReadLine();

            int age = GetIntInput("Возраст: "); // Безопасный ввод
            double grade = GetDoubleInput("Средний балл: "); // Безопасный ввод

            // Создание нового студента и добавление его в список
            students.Add(new Student { Name = name, Age = age, AverageGrade = grade });

            Console.WriteLine("Студент добавлен!");
            Console.ReadKey();
        }

        // 3. Редактировать
        static void EditStudent()
        {
            ShowAll(); // Вывод списка, чтобы было видно номера
            if (students.Count == 0) return; // Если пусто — выход

            int index = GetIntInput("Номер студента для редактирования: ") - 1; // -1, т.к. индексы с 0

            // Проверка, что такой номер существует
            if (index >= 0 && index < students.Count)
            {
                Console.Write("Новое имя: ");
                students[index].Name = Console.ReadLine();

                students[index].Age = GetIntInput("Новый возраст: ");
                students[index].AverageGrade = GetDoubleInput("Новый средний балл: ");

                Console.WriteLine("Обновлено!");
            }
            Console.ReadKey();
        }

        // 4. Удалить
        static void DeleteStudent()
        {
            ShowAll();
            if (students.Count == 0) return;

            int index = GetIntInput("Номер студента для удаления: ") - 1;

            // Проверка корректности индекса
            if (index >= 0 && index < students.Count)
            {
                students.RemoveAt(index); // Удаление по индексу
                Console.WriteLine("Удалено!");
            }
            Console.ReadKey();
        }

        // 5. Поиск
        static void SearchStudent()
        {
            Console.Write("Введите имя для поиска: ");
            string query = Console.ReadLine().ToLower(); // В нижний регистр для поиска без учета регистра

            // Поиск всех, чье имя содержит введенный текст
            var found = students.Where(s => s.Name.ToLower().Contains(query)).ToList();

            Console.WriteLine("\nРезультаты поиска:");
            if (found.Count == 0) Console.WriteLine("Ничего не найдено.");

            foreach (var s in found) Console.WriteLine(s);
            Console.ReadKey();
        }

        // 6. Сортировка
        static void SortStudents()
        {
            // Сортировка по убыванию среднего балла
            students = students.OrderByDescending(s => s.AverageGrade).ToList();

            Console.WriteLine("Отсортировано по убыванию среднего балла!");
            ShowAll();
        }

        // Работа  файлом

        // Сохранение в файл
        static void SaveToFile()
        {
            // Превращение каждого студента в строку "Имя|Возраст|Балл"
            // Балл сохраняется с точкой вместо запятой, чтобы файл был кроссплатформенным
            var lines = students.Select(s =>
                $"{s.Name}|{s.Age}|{s.AverageGrade.ToString().Replace(',', '.')}");

            // Запись всех строк в файл
            File.WriteAllLines(FileName, lines);
        }

        // Загрузка из файла
        static void LoadFromFile()
        {
            if (!File.Exists(FileName)) return; // Если файла нет — ничего не происходит

            // Чтение файла построчно
            foreach (var line in File.ReadAllLines(FileName))
            {
                // Разбив строки по символу '|'
                var parts = line.Split('|');

                if (parts.Length == 3) // Проверка, что там ровно 3 части
                {
                    students.Add(new Student
                    {
                        Name = parts[0],
                        Age = int.Parse(parts[1]),
                        // При загрузке меняем точку на запятую, чтобы парсинг прошел успешно
                        AverageGrade = double.Parse(parts[2].Replace('.', ','))
                    });
                }
            }
        }
    }
}