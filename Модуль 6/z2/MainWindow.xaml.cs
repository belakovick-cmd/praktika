using System;
using System.Collections.Generic;      
using System.Collections.ObjectModel;  
using System.IO;                        
using System.Linq;                      
using System.Text;                      
using System.Windows;
using System.Windows.Controls;

namespace z2
{
    // Класс задачи
    public class Task
    {
        public int Id { get; set; }              // уникальный ID
        public string Title { get; set; }        // название
        public string Description { get; set; }  // описание
        public string Priority { get; set; }     // приоритет
        public DateTime DueDate { get; set; }    // срок
        public bool IsCompleted { get; set; }    // выполнена?
    }

    public partial class MainWindow : Window
    {
        // Список задач
        private ObservableCollection<Task> tasks = new ObservableCollection<Task>();

        // Выбранная задача
        private Task selectedTask = null;

        private const string FileName = "tasks.txt";

        // Разделитель полей в файле
        private const char Separator = '|';

        // Конструктор
        public MainWindow()
        {
            InitializeComponent();

            // Привязка списка к DataGrid
            dgTasks.ItemsSource = tasks;

            // Загрузка задачи из файла
            LoadFromFile();
        }

        // Сохранение при закрытии
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            SaveToFile();
            base.OnClosing(e);
        }

        // Сохранение в файл
        private void SaveToFile()
        {
            // Формирование строки для записи
            var lines = new List<string>();

            foreach (var task in tasks)
            {
                // Экранирование разделителя | в полях на § 
                string title = task.Title.Replace("|", "§");
                string desc = (task.Description ?? "").Replace("|", "§");
                string priority = task.Priority.Replace("|", "§");

                // Формат: Id|Название|Описание|Приоритет|Дата|Выполнена
                string line = string.Join(Separator.ToString(),
                    task.Id,
                    title,
                    desc,
                    priority,
                    task.DueDate.ToString("yyyy-MM-dd"),
                    task.IsCompleted ? "1" : "0");

                lines.Add(line);
            }

            // Запист в файл в UTF-8 (для русских букв)
            File.WriteAllLines(FileName, lines, Encoding.UTF8);
        }

        // Загрузка из файла
        private void LoadFromFile()
        {
            // Очистка текущего списка
            tasks.Clear();

            // Если файла нет — ничего не загружать
            if (!File.Exists(FileName)) return;

            // Чтение всех строк
            string[] lines = File.ReadAllLines(FileName, Encoding.UTF8);

            foreach (string line in lines)
            {
                // Пропуск пустых строк
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Разбив по разделителю
                string[] parts = line.Split(Separator);

                // Должно быть ровно 6 частей
                if (parts.Length != 6) continue;

                try
                {
                    // Восстанавление экранированных символов
                    string title = parts[1].Replace("§", "|");
                    string desc = parts[2].Replace("§", "|");
                    string priority = parts[3].Replace("§", "|");

                    tasks.Add(new Task
                    {
                        Id = int.Parse(parts[0]),
                        Title = title,
                        Description = desc,
                        Priority = priority,
                        DueDate = DateTime.Parse(parts[4]),
                        IsCompleted = parts[5] == "1"
                    });
                }
                catch
                {
                    // Битая строка — пропуск
                }
            }
        }

        // Следующий ID
        private int GetNextId()
        {
            // Если задач нет — начало с 1
            if (tasks.Count == 0) return 1;

            // Иначе — максимальный ID + 1
            return tasks.Max(t => t.Id) + 1;
        }

        // Кнопка "Добавить"
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название задачи!", "Ошибка");
                return;
            }

            var task = new Task
            {
                Id = GetNextId(),
                Title = txtTitle.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Priority = GetSelectedPriority(),
                DueDate = dpDueDate.SelectedDate ?? DateTime.Today,
                IsCompleted = chkCompleted.IsChecked == true
            };

            tasks.Add(task);

            SaveToFile();   // Сохранение сразу
            ClearForm();
        }

        // Кнопка "Обновить"
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Выберите задачу в таблице!", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название задачи!", "Ошибка");
                return;
            }

            // Обновление поля выбранной задачи
            selectedTask.Title = txtTitle.Text.Trim();
            selectedTask.Description = txtDescription.Text.Trim();
            selectedTask.Priority = GetSelectedPriority();
            selectedTask.DueDate = dpDueDate.SelectedDate ?? DateTime.Today;
            selectedTask.IsCompleted = chkCompleted.IsChecked == true;

            // Обновление DataGrid
            dgTasks.ItemsSource = null;
            dgTasks.ItemsSource = tasks;

            SaveToFile();   // Сохранение
            ClearForm();
        }

        // Кнопка "Удалить"
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTask == null)
            {
                MessageBox.Show("Выберите задачу в таблице!", "Ошибка");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить задачу \"{selectedTask.Title}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                tasks.Remove(selectedTask);
                SaveToFile();   // Сохранение
                ClearForm();
            }
        }

        // Кнопка "Очистить"
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        // Очистка формы
        private void ClearForm()
        {
            txtTitle.Clear();
            txtDescription.Clear();
            cmbPriority.SelectedIndex = 1;
            dpDueDate.SelectedDate = null;
            chkCompleted.IsChecked = false;

            selectedTask = null;
            dgTasks.SelectedItem = null;
        }

        // Выбор строки
        private void dgTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedTask = dgTasks.SelectedItem as Task;

            if (selectedTask != null)
            {
                txtTitle.Text = selectedTask.Title;
                txtDescription.Text = selectedTask.Description;
                cmbPriority.SelectedIndex = PriorityToIndex(selectedTask.Priority);
                dpDueDate.SelectedDate = selectedTask.DueDate;
                chkCompleted.IsChecked = selectedTask.IsCompleted;
            }
        }

        // Получение приоритета из COMBOBOX 
        private string GetSelectedPriority()
        {
            if (cmbPriority.SelectedItem is ComboBoxItem item)
                return item.Content.ToString();
            return "Средний";
        }

        // Приоритет -> индекс
        private int PriorityToIndex(string priority)
        {
            switch (priority)
            {
                case "Низкий": return 0;
                case "Средний": return 1;
                case "Высокий": return 2;
                default: return 1;
            }
        }
    }
}