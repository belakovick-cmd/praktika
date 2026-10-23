using System;
using System.Collections.Generic;
using System.IO;      
using System.Linq;    
using System.Windows;
using System.Windows.Controls;

namespace z2
{
    // Модели данных
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class TaskItem
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int EmployeeId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        // Для отображения в таблице
        public string ProjectName { get; set; }
        public string EmployeeName { get; set; }
    }

    public partial class MainWindow : Window
    {
        // Имена файлов базы данных
        private const string ProjectsFile = "projects_db.txt";
        private const string EmployeesFile = "employees_db.txt";
        private const string TasksFile = "tasks_db.txt";

        // Списки 
        private List<Employee> employees = new List<Employee>();
        private List<Project> projects = new List<Project>();
        private List<TaskItem> tasks = new List<TaskItem>();

        public MainWindow()
        {
            InitializeComponent();

            // 1. Загрузка "БД" при старте
            LoadDatabase();
            RefreshAll();
        }

        // База данных
        // Загрузка всех данных из файлов
        private void LoadDatabase()
        {
            // Загрузка проектов
            if (File.Exists(ProjectsFile))
            {
                foreach (var line in File.ReadAllLines(ProjectsFile))
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                        projects.Add(new Project { Id = int.Parse(parts[0]), Title = parts[1] });
                }
            }

            // Загрузка сотрудников
            if (File.Exists(EmployeesFile))
            {
                foreach (var line in File.ReadAllLines(EmployeesFile))
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                        employees.Add(new Employee { Id = int.Parse(parts[0]), Name = parts[1] });
                }
            }

            // Загрузка задач
            if (File.Exists(TasksFile))
            {
                foreach (var line in File.ReadAllLines(TasksFile))
                {
                    var parts = line.Split('|');
                    if (parts.Length == 5)
                        tasks.Add(new TaskItem
                        {
                            Id = int.Parse(parts[0]),
                            ProjectId = int.Parse(parts[1]),
                            EmployeeId = int.Parse(parts[2]),
                            Description = parts[3],
                            Status = parts[4]
                        });
                }
            }

            // Если файлов нет, создание тестовых данных
            if (projects.Count == 0 && employees.Count == 0)
            {
                projects.Add(new Project { Id = 1, Title = "Разработка сайта" });
                employees.Add(new Employee { Id = 1, Name = "Иван Иванов" });
                SaveDatabase(); // Сразу сохранение в файлы
            }
        }

        // Сохранение всех данных в файлы
        private void SaveDatabase()
        {
            // Сохранение проектов "Id|Title"
            File.WriteAllLines(ProjectsFile, projects.Select(p => $"{p.Id}|{p.Title}"));

            // Сохранение сотрудников "Id|Name"
            File.WriteAllLines(EmployeesFile, employees.Select(e => $"{e.Id}|{e.Name}"));

            // Сохранение задач "Id|ProjectId|EmployeeId|Description|Status"
            File.WriteAllLines(TasksFile, tasks.Select(t =>
                $"{t.Id}|{t.ProjectId}|{t.EmployeeId}|{t.Description}|{t.Status}"));
        }

        // Обновление интерфейса
        private void RefreshAll()
        {
            dgProjects.ItemsSource = null;
            dgProjects.ItemsSource = projects;

            dgEmployees.ItemsSource = null;
            dgEmployees.ItemsSource = employees;

            UpdateTasksView();

            cmbTaskProject.ItemsSource = null;
            cmbTaskProject.ItemsSource = projects;

            cmbTaskEmployee.ItemsSource = null;
            cmbTaskEmployee.ItemsSource = employees;
        }

        private void UpdateTasksView()
        {
            // Подтягивание имён для таблицы
            foreach (var t in tasks)
            {
                t.ProjectName = projects.FirstOrDefault(p => p.Id == t.ProjectId)?.Title ?? "Неизвестно";
                t.EmployeeName = employees.FirstOrDefault(e => e.Id == t.EmployeeId)?.Name ?? "Не назначен";
            }

            dgTasks.ItemsSource = null;
            dgTasks.ItemsSource = tasks;
        }

        // Добавление
        private void BtnAddProject_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProjectName.Text)) return;

            int newId = projects.Any() ? projects.Max(p => p.Id) + 1 : 1;
            projects.Add(new Project { Id = newId, Title = txtProjectName.Text });

            txtProjectName.Clear();
            SaveDatabase(); // Сохранение в файл
            RefreshAll();
        }

        private void BtnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmployeeName.Text)) return;

            int newId = employees.Any() ? employees.Max(emp => emp.Id) + 1 : 1;
            employees.Add(new Employee { Id = newId, Name = txtEmployeeName.Text });

            txtEmployeeName.Clear();
            SaveDatabase(); // Сохранение в файл
            RefreshAll();
        }

        private void BtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskDesc.Text)) return;
            if (cmbTaskProject.SelectedValue == null || cmbTaskEmployee.SelectedValue == null) return;

            int newId = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
            string status = (cmbTaskStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            tasks.Add(new TaskItem
            {
                Id = newId,
                Description = txtTaskDesc.Text,
                ProjectId = (int)cmbTaskProject.SelectedValue,
                EmployeeId = (int)cmbTaskEmployee.SelectedValue,
                Status = status
            });

            txtTaskDesc.Clear();
            SaveDatabase(); // Сохранение в файл
            UpdateTasksView();
        }

        // Редактирование
        // Двойной клик по задаче для изменения статуса
        private void DgTasks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgTasks.SelectedItem is TaskItem task)
            {
                // Создание окна
                Window editWindow = new Window
                {
                    Title = "Редактировать задачу",
                    Width = 400,
                    Height = 350,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this,
                    ResizeMode = ResizeMode.NoResize
                };

                StackPanel panel = new StackPanel { Margin = new Thickness(15) };

                // Поле: Описание
                panel.Children.Add(new TextBlock { Text = "Описание задачи:" });
                TextBox txtDesc = new TextBox { Text = task.Description, Margin = new Thickness(0, 5, 0, 15) };
                panel.Children.Add(txtDesc);

                // Поле: Статус
                panel.Children.Add(new TextBlock { Text = "Статус:" });
                ComboBox cmbStatus = new ComboBox { Margin = new Thickness(0, 5, 0, 15) };
                cmbStatus.Items.Add("Новая");
                cmbStatus.Items.Add("В работе");
                cmbStatus.Items.Add("Выполнена");
                cmbStatus.SelectedItem = task.Status;
                panel.Children.Add(cmbStatus);

                // Поле: Исполнитель
                panel.Children.Add(new TextBlock { Text = "Исполнитель:" });
                ComboBox cmbEmp = new ComboBox { Margin = new Thickness(0, 5, 0, 20) };
                cmbEmp.ItemsSource = employees;
                cmbEmp.DisplayMemberPath = "Name";
                cmbEmp.SelectedValuePath = "Id";
                cmbEmp.SelectedValue = task.EmployeeId;
                panel.Children.Add(cmbEmp);

                // Кнопка Сохранить
                Button btnSave = new Button { Content = "Сохранить", Width = 120, Height = 30, HorizontalAlignment = HorizontalAlignment.Center };
                btnSave.Click += (s, ev) =>
                {
                    task.Description = txtDesc.Text;
                    task.Status = cmbStatus.SelectedItem.ToString();
                    if (cmbEmp.SelectedValue != null)
                        task.EmployeeId = (int)cmbEmp.SelectedValue;

                    SaveDatabase(); // Сохранение изменений в файл!
                    UpdateTasksView();
                    editWindow.Close();
                };
                panel.Children.Add(btnSave);

                editWindow.Content = panel;
                editWindow.ShowDialog();
            }
        }

        // Удаление
        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is TaskItem task)
            {
                tasks.Remove(task);
                SaveDatabase(); // Сохраняем в файл
                UpdateTasksView();
            }
        }

        // Отчёты
        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            string report = "Отчёт по проектам\r\n\r\n";

            if (!projects.Any())
            {
                report += "Проектов пока нет.";
            }
            else
            {
                foreach (var proj in projects)
                {
                    var projTasks = tasks.Where(t => t.ProjectId == proj.Id).ToList();

                    report += $"Проект: {proj.Title}\r\n";
                    report += $"  Всего задач: {projTasks.Count}\r\n";
                    report += $"  Выполнено: {projTasks.Count(t => t.Status == "Выполнена")}\r\n";
                    report += $"  В работе: {projTasks.Count(t => t.Status == "В работе")}\r\n";
                    report += $"  Не начато: {projTasks.Count(t => t.Status == "Новая")}\r\n\r\n";
                }
            }

            txtReport.Text = report;
        }
    }
}