using System;

// Класс-издатель
class Notification
{
    // События 
    public event EventHandler<string> MessageReceived;   // сообщение
    public event EventHandler<string> CallReceived;      // звонок
    public event EventHandler<string> EmailReceived;     // письмо

    // Метод, генерирующий событие "сообщение"
    public void SendMessage(string text)
    {
        Console.WriteLine($"[Уведомление] Сообщение: {text}");
        MessageReceived?.Invoke(this, text);
    }

    // Метод, генерирующий событие "звонок"
    public void SendCall(string number)
    {
        Console.WriteLine($"[Уведомление] Звонок: {number}");
        CallReceived?.Invoke(this, number);
    }

    // Метод, генерирующий событие "письмо"
    public void SendEmail(string subject)
    {
        Console.WriteLine($"[Уведомление] Письмо: {subject}");
        EmailReceived?.Invoke(this, subject);
    }
}

class Program
{
    static void Main()
    {
        // Создание издателя
        Notification notification = new Notification();

        // Подписка на события 
        notification.MessageReceived += (s, text) =>
            Console.WriteLine($"  [Telegram] Получено: {text}");

        notification.CallReceived += (s, number) =>
            Console.WriteLine($"  [Телефон] Звонит: {number}");

        notification.EmailReceived += (s, subject) =>
            Console.WriteLine($"  [Почта] Письмо: {subject}");

        // Генерация событий
        notification.SendMessage("Привет!");
        notification.SendCall("+375 29 123-45-67");
        notification.SendEmail("Отчёт за квартал");

        Console.ReadKey();
    }
}