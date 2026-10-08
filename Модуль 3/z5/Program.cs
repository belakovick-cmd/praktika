using System;

// Делегат для метода сортировки: принимает массив, сортирует его
delegate void SortMethod(int[] array);

class Program
{
    // Методы сортировки
        // Сортировка пузырьком
    static void BubbleSort(int[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = 0; j < array.Length - 1 - i; j++)
            {
                if (array[j] > array[j + 1])
                {
                    // Обмен значениями
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                }
            }
        }
    }

    // Быстрая сортировка (QuickSort)
    static void QuickSort(int[] array)
    {
        QuickSortRecursive(array, 0, array.Length - 1);
    }

    // Вспомогательный метод для QuickSort
    static void QuickSortRecursive(int[] array, int left, int right)
    {
        if (left >= right) return;

        // Опорный элемент — средний
        int pivot = array[(left + right) / 2];
        int i = left, j = right;

        while (i <= j)
        {
            while (array[i] < pivot) i++;
            while (array[j] > pivot) j--;

            if (i <= j)
            {
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
        }

        // Рекурсивная сортировка левой и правой части
        QuickSortRecursive(array, left, j);
        QuickSortRecursive(array, i, right);
    }

    // Сортировка выбором
    static void SelectionSort(int[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int minIdx = i;
            for (int j = i + 1; j < array.Length; j++)
            {
                if (array[j] < array[minIdx])
                    minIdx = j;
            }

            // Обмен
            int temp = array[i];
            array[i] = array[minIdx];
            array[minIdx] = temp;
        }
    }

    // Вспомогательные методы
    // Вывод массива
    static void PrintArray(int[] array)
    {
        Console.WriteLine("  " + string.Join(", ", array));
    }

    // Создание случайного массива
    static int[] CreateRandomArray(int size)
    {
        Random random = new Random();
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
            array[i] = random.Next(1, 101);   // числа от 1 до 100
        return array;
    }

    static void Main()
    {
        // Массив делегатов и их названий
        SortMethod[] methods =
        {
            BubbleSort,
            QuickSort,
            SelectionSort
        };

        string[] names =
        {
            "Сортировка пузырьком",
            "Быстрая сортировка",
            "Сортировка выбором"
        };

        // Меню выбора
        Console.WriteLine("Выберите метод сортировки:");
        for (int i = 0; i < methods.Length; i++)
            Console.WriteLine($"  {i + 1} — {names[i]}");

        Console.Write("\nВыбор: ");
        int choice = int.Parse(Console.ReadLine()) - 1;

        // Выбор делегата
        SortMethod sort = methods[choice];

        // Создание массива
        int[] data = CreateRandomArray(10);

        Console.WriteLine($"\nИсходный массив:");
        PrintArray(data);

        // Сортировка через делегата
        sort(data);

        Console.WriteLine($"\nРезультат ({names[choice]}):");
        PrintArray(data);

        Console.ReadKey();
    }
}