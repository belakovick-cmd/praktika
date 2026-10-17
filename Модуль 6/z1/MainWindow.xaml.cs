using System;                          
using System.Collections.ObjectModel;  
using System.IO;                        
using System.Linq;                      
using System.Windows;                   
using System.Windows.Controls;          
using System.ComponentModel;            

namespace z1
{
    // Класс сотрудника
    public class Employee
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
    }

    public partial class MainWindow : Window
    {
        // Список сотрудников 
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

        // Выбранный сотрудник
        private Employee selected = null;

        // Имя файла для сохранения 
        private const string FileName = "employees.txt";

        // Конструктор
        public MainWindow()
        {
            InitializeComponent();

            // Привязывание списка к DataGrid
            dgEmployees.ItemsSource = employees;

            // Загрузка данных из файла (если он есть)
            LoadFromFile();

            // Если файла не было — добавлются примеры
            if (employees.Count == 0)
            {
                employees.Add(new Employee
                {
                    FullName = "Иванов Иван Иванович",
                    Position = "Менеджер",
                    Department = "Продажи",
                    Salary = 50000,
                    HireDate = new DateTime(2023, 1, 15)
                });

                employees.Add(new Employee
                {
                    FullName = "Петрова Мария Сергеевна",
                    Position = "Бухгалтер",
                    Department = "Финансы",
                    Salary = 60000,
                    HireDate = new DateTime(2022, 5, 10)
                });
            }
        }

        // Сохранение при закрытиии окна
        protected override void OnClosing(CancelEventArgs e)
        {
            SaveToFile();          // сохранение данных
            base.OnClosing(e);
        }

        // Сохранение в файл
        private void SaveToFile()
        {
            // Преобразование каждого сотрудника в строку формата: ФИО|Должность|Отдел|Зарплата|Дата
            var lines = employees.Select(e =>
                $"{e.FullName}|{e.Position}|{e.Department}|{e.Salary}|{e.HireDate:yyyy-MM-dd}");

            // Запись всех строк в файл 
            File.WriteAllLines(FileName, lines);
        }

        // Загрузка из файла
        private void LoadFromFile()
        {
            // Если файла нет — выход
            if (!File.Exists(FileName)) return;

            // Чтение всех строк
            foreach (string line in File.ReadAllLines(FileName))
            {
                // Разбив строк по символу '|'
                string[] parts = line.Split('|');

                // Пропуск некорректных строк
                if (parts.Length != 5) continue;

                // Парсим и добавляем сотрудника
                try
                {
                    employees.Add(new Employee
                    {
                        FullName = parts[0],
                        Position = parts[1],
                        Department = parts[2],
                        Salary = decimal.Parse(parts[3]),
                        HireDate = DateTime.Parse(parts[4])
                    });
                }
                catch
                {
                    // Если строка битая — пропуск
                }
            }
        }

        // Кнопка "Добавить"
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО!", "Ошибка");
                return;
            }

            if (!decimal.TryParse(txtSalary.Text, out decimal salary))
            {
                MessageBox.Show("Неверный формат зарплаты!", "Ошибка");
                return;
            }

            employees.Add(new Employee
            {
                FullName = txtFullName.Text.Trim(),
                Position = txtPosition.Text.Trim(),
                Department = txtDepartment.Text.Trim(),
                Salary = salary,
                HireDate = dpHireDate.SelectedDate ?? DateTime.Today
            });

            SaveToFile();    // Сохранение сразу после добавления
            ClearForm();
        }

        // Кнопка "Редактировать"
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Выберите сотрудника!", "Ошибка");
                return;
            }

            if (!decimal.TryParse(txtSalary.Text, out decimal salary))
            {
                MessageBox.Show("Неверный формат зарплаты!", "Ошибка");
                return;
            }

            selected.FullName = txtFullName.Text.Trim();
            selected.Position = txtPosition.Text.Trim();
            selected.Department = txtDepartment.Text.Trim();
            selected.Salary = salary;
            selected.HireDate = dpHireDate.SelectedDate ?? DateTime.Today;

            // Обновление DataGrid
            dgEmployees.ItemsSource = null;
            dgEmployees.ItemsSource = employees;

            SaveToFile();    // Сохранение после изменения
            ClearForm();
        }

        // Кнопка "Удалить"
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null)
            {
                MessageBox.Show("Выберите сотрудника!", "Ошибка");
                return;
            }

            var result = MessageBox.Show(
                $"Удалить \"{selected.FullName}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                employees.Remove(selected);
                SaveToFile();    // Сохранение после удаления
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
            txtFullName.Clear();
            txtPosition.Clear();
            txtDepartment.Clear();
            txtSalary.Clear();
            dpHireDate.SelectedDate = null;

            selected = null;
            dgEmployees.SelectedItem = null;
        }

        // Выбор строки
        private void dgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = dgEmployees.SelectedItem as Employee;

            if (selected != null)
            {
                txtFullName.Text = selected.FullName;
                txtPosition.Text = selected.Position;
                txtDepartment.Text = selected.Department;
                txtSalary.Text = selected.Salary.ToString();
                dpHireDate.SelectedDate = selected.HireDate;
            }
        }
    }
}