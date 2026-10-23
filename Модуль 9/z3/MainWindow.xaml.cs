using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace z3
{
    // Модель данных
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Priority { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
    }

    public partial class MainWindow : Window
    {
        // База данных (файл)
        private const string FileName = "tasks_db.txt";
        private List<TaskItem> tasks = new List<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();

            LoadDatabase(); // Загрузка задачи из файла при старте
            RefreshGrid();
        }

        // База данных (Файлы)
        // Сохранение в файл (текстовая БД)
        private void SaveDatabase()
        {
            // Формат строки: Id|Title|Priority|Deadline|IsCompleted
            var lines = tasks.Select(t =>
                $"{t.Id}|{t.Title}|{t.Priority}|{t.Deadline:yyyy-MM-dd}|{(t.IsCompleted ? 1 : 0)}");

            File.WriteAllLines(FileName, lines);
        }

        // Загрузка из файла
        private void LoadDatabase()
        {
            if (!File.Exists(FileName)) return;

            foreach (var line in File.ReadAllLines(FileName))
            {
                var parts = line.Split('|');
                if (parts.Length == 5)
                {
                    tasks.Add(new TaskItem
                    {
                        Id = int.Parse(parts[0]),
                        Title = parts[1],
                        Priority = parts[2],
                        Deadline = DateTime.Parse(parts[3]),
                        IsCompleted = parts[4] == "1"
                    });
                }
            }
        }

        // Работа с интерфейсом
        // Обновление таблицы
        private void RefreshGrid()
        {
            dgTasks.ItemsSource = null;
            dgTasks.ItemsSource = tasks;
        }

        // Добавление задачи
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) return;

            int newId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
            string priority = (cmbPriority.SelectedItem as ComboBoxItem)?.Content.ToString();

            tasks.Add(new TaskItem
            {
                Id = newId,
                Title = txtTitle.Text,
                Priority = priority,
                Deadline = dpDeadline.SelectedDate ?? DateTime.Today.AddDays(1), // Если не выбрана, то завтра
                IsCompleted = false
            });

            txtTitle.Clear();
            SaveDatabase(); // Сохранение в файл
            RefreshGrid();
        }

        // "Отметить выполненной"
        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is TaskItem task)
            {
                task.IsCompleted = true; // Смена статуса
                SaveDatabase();          // Сохранение в файл
                RefreshGrid();           // Обновление таблицы
            }
            else
            {
                MessageBox.Show("Выберите задачу в таблице!", "Ошибка");
            }
        }

        // Удаление
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is TaskItem task)
            {
                tasks.Remove(task);
                SaveDatabase(); // Сохранение в файл
                RefreshGrid();
            }
        }

        // Уведомления

        private void BtnCheckDeadlines_Click(object sender, RoutedEventArgs e)
        {
            // Поиск задачи, которые НЕ выполнены и до срока осталось 2 дня или меньше
            var urgent = tasks.Where(t => !t.IsCompleted && (t.Deadline - DateTime.Today).TotalDays <= 2).ToList();

            if (urgent.Any())
            {
                string msg = "ВНИМАНИЕ! Приближается срок выполнения:\n\n";
                foreach (var t in urgent)
                    msg += $"- {t.Title} (до {t.Deadline:dd.MM.yyyy}, приоритет: {t.Priority})\n";

                MessageBox.Show(msg, "Напоминание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Срочных задач нет.", "Уведомления", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}