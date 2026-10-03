using System;

// Класс аргументов события
class TemperatureChangedEventArgs : EventArgs
{
    // Хранение старой и новой температуры
    public double OldTemperature { get; }
    public double NewTemperature { get; }

    public TemperatureChangedEventArgs(double oldTemp, double newTemp)
    {
        OldTemperature = oldTemp;
        NewTemperature = newTemp;
    }
}

// Издатель события
class TemperatureSensor
{
    private double temperature; // текущая температура

    public event EventHandler<TemperatureChangedEventArgs> TemperatureChanged;

    public double Temperature
    {
        get => temperature; // возвращение 
        set
        {
            // Событие срабатывает только если значение реально изменилось
            if (temperature != value)
            {
                double old = temperature; // запоминаем старое
                temperature = value;      // записываем новое

                TemperatureChanged?.Invoke(this,
                    new TemperatureChangedEventArgs(old, value));
            }
        }
    }
}

// Подписчик
class Thermostat
{
    private double threshold; // порог температуры
    private bool isHeatingOn; // состояние отопления

    public Thermostat(double threshold)
    {
        this.threshold = threshold;
        isHeatingOn = false; // изначально выключено
    }

    // Обработчик события
    public void OnTemperatureChanged(object sender, TemperatureChangedEventArgs e)
    {
        Console.WriteLine($"  [Термостат] Температура: {e.OldTemperature}°C -> {e.NewTemperature}°C");

        // Если стало холоднее порога и отопление выключено — включаем
        if (e.NewTemperature < threshold && !isHeatingOn)
        {
            isHeatingOn = true;
            Console.WriteLine($"  [Термостат] Отопление ВКЛЮЧЕНО (ниже {threshold}°C)");
        }
        // Если достигли порога и отопление включено — выключаем
        else if (e.NewTemperature >= threshold && isHeatingOn)
        {
            isHeatingOn = false;
            Console.WriteLine($"  [Термостат] Отопление ВЫКЛЮЧЕНО (достигнуто {threshold}°C)");
        }
    }
}

class Program
{
    static void Main()
    {
        // Создание издателя и подписчика
        TemperatureSensor sensor = new TemperatureSensor();
        Thermostat thermostat = new Thermostat(22.0);

        // Подписка на событие
        // += добавляет обработчик в список подписчиков
        sensor.TemperatureChanged += thermostat.OnTemperatureChanged;

        // Изменение температуры -> Событие срабатывает
        Console.WriteLine("Устанавливаем температуру 18°C:");
        sensor.Temperature = 18; // сеттер вызовет событие

        Console.WriteLine("\nУстанавливаем температуру 25°C:");
        sensor.Temperature = 25;

        Console.WriteLine("\nУстанавливаем температуру 20°C:");
        sensor.Temperature = 20;

        Console.ReadKey();
    }
}