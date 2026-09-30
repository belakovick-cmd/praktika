using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите размер матрицы N: ");
        int n = int.Parse(Console.ReadLine());

        Random random = new Random();
        int[,] matrix = new int[n, n];

        // Заполнение матрицы случайными числами из [-50, 50]
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                matrix[i, j] = random.Next(-50, 51);

        Console.WriteLine("\nИсходная матрица:");
        PrintMatrix(matrix, n);
        PrintSums(matrix, n);

        // Подсчёт суммы строк
        int[] sums = new int[n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
                sums[i] += matrix[i, j];
        }

        // Создание массивов индексов строк и сортирование по суммам
        int[] rowIndices = new int[n];
        for (int i = 0; i < n; i++)
            rowIndices[i] = i;

        Array.Sort(rowIndices, (x, y) => sums[x].CompareTo(sums[y]));

        // Формирование отсортированной матрицы
        int[,] sortedMatrix = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                sortedMatrix[i, j] = matrix[rowIndices[i], j];

        Console.WriteLine("\nМатрица после сортировки строк по суммам:");
        PrintMatrix(sortedMatrix, n);
        PrintSums(sortedMatrix, n);

        Console.ReadKey();
    }

    // Вывод матрицы
    static void PrintMatrix(int[,] matrix, int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
                Console.Write($"{matrix[i, j],6}");
            Console.WriteLine();
        }
    }

    // Вывод сумм строк
    static void PrintSums(int[,] matrix, int n)
    {
        Console.Write("Суммы строк: ");
        for (int i = 0; i < n; i++)
        {
            int sum = 0;
            for (int j = 0; j < n; j++)
                sum += matrix[i, j];
            Console.Write($"{sum,6}");
        }
        Console.WriteLine();
    }
}