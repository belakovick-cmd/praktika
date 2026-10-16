using System.Collections.Generic;   

namespace z3
{
    // Логика работы с задачами
    public static class TaskLogic
    {
        // Список задач
        public static List<string> Tasks = new List<string>();

        // Добавить задачу
        // Возвращение true, если добавлено; false — если пусто
        public static bool AddTask(string task)
        {
            // Удаление лишних пробелов
            task = task?.Trim();

            // Проверка: не пустая
            if (string.IsNullOrEmpty(task))
                return false;

            // Добавление в список
            Tasks.Add(task);
            return true;
        }

        // Удалить задачу по индексу
        public static void DeleteTask(int index)
        {
            // Проверка: индекс корректен
            if (index < 0 || index >= Tasks.Count)
                return;

            Tasks.RemoveAt(index);
        }

        // Отметить/снять отметку выполнения
        public static void ToggleComplete(int index)
        {
            // Проверка: индекс корректен
            if (index < 0 || index >= Tasks.Count)
                return;

            string task = Tasks[index];

            // Если ещё не отмечено — отметка [✓]
            if (!task.StartsWith("[✓]"))
            {
                Tasks[index] = "[✓] " + task;
            }
            else
            {
                // Если отмечено — убрать [✓]
                Tasks[index] = task.Replace("[✓] ", "");
            }
        }

        // Получение всех задач
        public static List<string> GetAll()
        {
            return Tasks;
        }

        // Очищение всего
        public static void ClearAll()
        {
            Tasks.Clear();
        }
    }
}