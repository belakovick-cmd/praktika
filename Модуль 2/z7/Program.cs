using System;

struct Student
{
    public string LastName;      // фамилия и инициалы
    public string GroupNumber;   // номер группы
    public int[] Grades;         // массив из 5 оценок

    // Средний бал
    public double GetAverageGrade()
    {
        int sum = 0;
        foreach (int g in Grades) sum += g;      // сумма всех оценок
        return (double)sum / Grades.Length;      // деление на количество
    }

    // Проверка
    public bool IsExcellentOrGood()
    {
        foreach (int g in Grades)
            if (g < 4) return false; // ннахождение плохой оценка - false
        return true; // все оценки >= 4
    }

    public void PrintInfo()
    {
        Console.WriteLine($"{LastName} (группа {GroupNumber}) — средний балл: {GetAverageGrade():F2}");
    }
}

class Program
{
    static void Main()
    {
        // Массив из студентов
        Student[] students = new Student[4];

        // Заполнение тестовыми данными через вспомогательный метод
        students[0] = CreateStudent("Соколов К.В.", "П24", new int[] { 5, 5, 4, 5, 5 });
        students[1] = CreateStudent("Сидоренко С.В.", "П24", new int[] { 3, 4, 4, 3, 4 });
        students[2] = CreateStudent("Сидоров С.С.", "П24", new int[] { 5, 5, 5, 5, 5 });
        students[3] = CreateStudent("Морозов М.М.", "П24", new int[] { 5, 4, 5, 4, 5 });

        // Сортировка пузырьком по среднему баллу
        for (int i = 0; i < students.Length - 1; i++)
        {
            for (int j = 0; j < students.Length - 1 - i; j++)
            {
                // Если текущий больше следующего — смена мест
                if (students[j].GetAverageGrade() > students[j + 1].GetAverageGrade())
                {
                    // Обмен через временную переменную
                    Student temp = students[j];
                    students[j] = students[j + 1];
                    students[j + 1] = temp;
                }
            }
        }

        // Вывод после сортировки
        Console.WriteLine("Студенты в порядке возрастания среднего балла:\n");
        foreach (Student s in students)
        {
            s.PrintInfo();
        }

        // Вывод только с 4 и 5
        Console.WriteLine("\nСтуденты с оценками только 4 и 5:\n");
        foreach (Student s in students)
        {
            if (s.IsExcellentOrGood()) // Проверка условия
            {
                Console.WriteLine($"{s.LastName} — группа {s.GroupNumber}");
            }
        }

        Console.ReadKey();
    }

    // Вспомогательный метод
    // Создание и возвращания студента с заданными полями
    static Student CreateStudent(string lastName, string group, int[] grades)
    {
        return new Student
        {
            LastName = lastName,   // инициализация поля
            GroupNumber = group,
            Grades = grades
        };
    }
}