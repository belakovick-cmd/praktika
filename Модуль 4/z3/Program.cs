using System;

// Интерфейс "Студент"
interface IStudent
{
    string Name { get; }              // ФИО студента
    int Course { get; }               // курс (1-4)
    double GetAverageGrade();         // средний балл
    string GetCourseInfo();           // информация о курсе
}

// Базовый класс с общими полями 
abstract class StudentBase : IStudent
{
    public string Name { get; protected set; }
    public int Course { get; protected set; }
    protected int[] Grades;           // оценки студента

    protected StudentBase(string name, int course, int[] grades)
    {
        Name = name;
        Course = course;
        Grades = grades;
    }

    // Средний балл 
    public virtual double GetAverageGrade()
    {
        if (Grades.Length == 0) return 0;
        int sum = 0;
        foreach (int g in Grades) sum += g;
        return (double)sum / Grades.Length;
    }

    // Информация о курсе 
    public abstract string GetCourseInfo();
}

// Студент 1 курса
class FirstYearStudent : StudentBase
{
    public FirstYearStudent(string name, int[] grades)
        : base(name, 1, grades) { }

    public override string GetCourseInfo()
    {
        return "1 курс - общие дисциплины";
    }
}

// Студент 2 курса
class SecondYearStudent : StudentBase
{
    public SecondYearStudent(string name, int[] grades)
        : base(name, 2, grades) { }

    public override string GetCourseInfo()
    {
        return "2 курс - профильные предметы";
    }
}

// Студент 3 курса
class ThirdYearStudent : StudentBase
{
    public ThirdYearStudent(string name, int[] grades)
        : base(name, 3, grades) { }

    public override string GetCourseInfo()
    {
        return "3 курс - специализация";
    }
}

// Студент 4 курса
class FourthYearStudent : StudentBase
{
    public FourthYearStudent(string name, int[] grades)
        : base(name, 4, grades) { }

    public override string GetCourseInfo()
    {
        return "4 курс - дипломная работа";
    }
}

class Program
{
    static void Main()
    {
        // Массив студентов 
        IStudent[] students =
        {
            new FirstYearStudent("Иванов И.И.",  new int[] { 4, 5, 4, 5 }),
            new SecondYearStudent("Петров П.П.", new int[] { 3, 4, 4, 3 }),
            new ThirdYearStudent("Сидоров С.С.", new int[] { 5, 5, 5, 5 }),
            new FourthYearStudent("Козлов К.К.", new int[] { 4, 4, 5, 5 })
        };

        Console.WriteLine("Студенты университета\n");

        double totalAvg = 0;

        foreach (IStudent s in students)
        {
            double avg = s.GetAverageGrade();
            totalAvg += avg;

            Console.WriteLine($"Студент: {s.Name}");
            Console.WriteLine($"  Курс: {s.Course}");
            Console.WriteLine($"  {s.GetCourseInfo()}");
            Console.WriteLine($"  Средний балл: {avg:F2}");
            Console.WriteLine();
        }

        Console.WriteLine($"Средний балл по университету: {totalAvg / students.Length:F2}");

        Console.ReadKey();
    }
}