using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace z4
{
    // Класс операции
    public class Transaction
    {
        public int Id { get; set; }              // ID
        public DateTime Date { get; set; }       // дата
        public string Type { get; set; }         // "Доход" или "Расход"
        public string Category { get; set; }     // категория
        public decimal Amount { get; set; }      // сумма
        public string Description { get; set; }  // описание

        // Вычисляемый цвет суммы 
        public string AmountColor => Type == "Доход" ? "Green" : "Red";
    }

    public partial class MainWindow : Window
    {
        // Все операции
        private ObservableCollection<Transaction> transactions = new ObservableCollection<Transaction>();

        // Отфильтрованные
        private ObservableCollection<Transaction> filteredTransactions = new ObservableCollection<Transaction>();

        // Выбранная операция
        private Transaction selectedTransaction = null;

        // Файл
        private const string FileName = "finance.txt";
        private const char Separator = '|';

        // Конструктор
        public MainWindow()
        {
            InitializeComponent();

            dgTransactions.ItemsSource = filteredTransactions;

            // Начальная дата — сегодня
            dpDate.SelectedDate = DateTime.Today;

            // Заполнение категории для доходов
            UpdateCategories();

            LoadFromFile();

            // Если файла не было — примеры
            if (transactions.Count == 0)
            {
                transactions.Add(new Transaction { Id = 1, Date = DateTime.Today.AddDays(-5), Type = "Доход", Category = "Зарплата", Amount = 50000, Description = "Зарплата за месяц" });
                transactions.Add(new Transaction { Id = 2, Date = DateTime.Today.AddDays(-3), Type = "Расход", Category = "Продукты", Amount = 3500, Description = "Покупка в магазине" });
                transactions.Add(new Transaction { Id = 3, Date = DateTime.Today.AddDays(-1), Type = "Расход", Category = "Транспорт", Amount = 500, Description = "Проездной" });
                SaveToFile();
            }

            ApplyFilter();
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
            var lines = new List<string>();

            foreach (var t in transactions)
            {
                // Экранирование |
                string category = t.Category.Replace("|", "§");
                string desc = (t.Description ?? "").Replace("|", "§");

                // Формат: Id|Дата|Тип|Категория|Сумма|Описание
                string line = string.Join(Separator.ToString(),
                    t.Id,
                    t.Date.ToString("yyyy-MM-dd"),
                    t.Type,
                    category,
                    t.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    desc);

                lines.Add(line);
            }

            File.WriteAllLines(FileName, lines, Encoding.UTF8);
        }

        // Загрузка из файла
        private void LoadFromFile()
        {
            transactions.Clear();

            if (!File.Exists(FileName)) return;

            string[] lines = File.ReadAllLines(FileName, Encoding.UTF8);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(Separator);
                if (parts.Length != 6) continue;

                try
                {
                    transactions.Add(new Transaction
                    {
                        Id = int.Parse(parts[0]),
                        Date = DateTime.Parse(parts[1]),
                        Type = parts[2],
                        Category = parts[3].Replace("§", "|"),
                        Amount = decimal.Parse(parts[4], System.Globalization.CultureInfo.InvariantCulture),
                        Description = parts[5].Replace("§", "|")
                    });
                }
                catch { /* пропускаем битую */ }
            }
        }

        // Слудующий ID
        private int GetNextId()
        {
            if (transactions.Count == 0) return 1;
            return transactions.Max(t => t.Id) + 1;
        }

        // Обновление итогов
        private void UpdateTotals()
        {
            decimal income = transactions
                .Where(t => t.Type == "Доход")
                .Sum(t => t.Amount);

            decimal expense = transactions
                .Where(t => t.Type == "Расход")
                .Sum(t => t.Amount);

            decimal balance = income - expense;

            lblIncome.Text = $"Доходы: {income:N2} руб.";
            lblExpense.Text = $"Расходы: {expense:N2} руб.";
            lblBalance.Text = $"Баланс: {balance:N2} руб.";

            // Цвет баланса
            if (balance >= 0)
                lblBalance.Foreground = System.Windows.Media.Brushes.Blue;
            else
                lblBalance.Foreground = System.Windows.Media.Brushes.Red;
        }

        // Обновление категории
        private void UpdateCategories()
        {
            string selectedType = GetSelectedType();
            cmbCategory.Items.Clear();

            if (selectedType == "Доход")
            {
                cmbCategory.Items.Add("Зарплата");
                cmbCategory.Items.Add("Премия");
                cmbCategory.Items.Add("Подарок");
                cmbCategory.Items.Add("Проценты");
                cmbCategory.Items.Add("Прочее");
            }
            else
            {
                cmbCategory.Items.Add("Продукты");
                cmbCategory.Items.Add("Транспорт");
                cmbCategory.Items.Add("Жильё");
                cmbCategory.Items.Add("Развлечения");
                cmbCategory.Items.Add("Здоровье");
                cmbCategory.Items.Add("Одежда");
                cmbCategory.Items.Add("Прочее");
            }

            cmbCategory.SelectedIndex = 0;
        }

        // Смена типа
        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Смена списка категорий в зависимости от типа
            if (cmbCategory != null)
                UpdateCategories();
        }

        // Фильтр
        private void ApplyFilter()
        {
            string search = txtSearch.Text.Trim().ToLower();
            filteredTransactions.Clear();

            foreach (var t in transactions)
            {
                if (string.IsNullOrEmpty(search) ||
                    t.Category.ToLower().Contains(search) ||
                    (t.Description ?? "").ToLower().Contains(search))
                {
                    filteredTransactions.Add(t);
                }
            }

            UpdateTotals();
        }

        // Поиск
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Кнопка "Показать все"
        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
        }

        // Кнопка "Добавить"
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            // Проверка суммы
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму (> 0)!", "Ошибка");
                return;
            }

            // Проверка категории
            string category = cmbCategory.Text?.Trim();
            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Введите категорию!", "Ошибка");
                return;
            }

            transactions.Add(new Transaction
            {
                Id = GetNextId(),
                Date = dpDate.SelectedDate ?? DateTime.Today,
                Type = GetSelectedType(),
                Category = category,
                Amount = amount,
                Description = txtDescription.Text.Trim()
            });

            SaveToFile();
            ApplyFilter();
            ClearForm();
        }

        // Кнопка "Редактировать"
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTransaction == null)
            {
                MessageBox.Show("Выберите операцию в таблице!", "Ошибка");
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму (> 0)!", "Ошибка");
                return;
            }

            string category = cmbCategory.Text?.Trim();
            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Введите категорию!", "Ошибка");
                return;
            }

            // Обновление полей
            selectedTransaction.Date = dpDate.SelectedDate ?? DateTime.Today;
            selectedTransaction.Type = GetSelectedType();
            selectedTransaction.Category = category;
            selectedTransaction.Amount = amount;
            selectedTransaction.Description = txtDescription.Text.Trim();

            SaveToFile();

            // Обновление DataGrid
            dgTransactions.ItemsSource = null;
            dgTransactions.ItemsSource = filteredTransactions;

            ApplyFilter();
            ClearForm();
        }

        // Кнопка "Удалить"
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedTransaction == null)
            {
                MessageBox.Show("Выберите операцию в таблице!", "Ошибка");
                return;
            }

            // Сохранение данных до ApplyFilter
            string info = $"{selectedTransaction.Type} — {selectedTransaction.Category} ({selectedTransaction.Amount:N2})";

            var result = MessageBox.Show(
                $"Удалить операцию?\n{info}",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                transactions.Remove(selectedTransaction);
                SaveToFile();
                ApplyFilter();
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
            dpDate.SelectedDate = DateTime.Today;
            cmbType.SelectedIndex = 0;
            cmbCategory.SelectedIndex = 0;
            txtAmount.Clear();
            txtDescription.Clear();

            selectedTransaction = null;
            dgTransactions.SelectedItem = null;
        }

        // Выбор операции
        private void dgTransactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Защита от обнуления при ApplyFilter
            if (dgTransactions.SelectedItem == null) return;

            selectedTransaction = dgTransactions.SelectedItem as Transaction;

            if (selectedTransaction != null)
            {
                dpDate.SelectedDate = selectedTransaction.Date;
                cmbType.SelectedIndex = selectedTransaction.Type == "Доход" ? 0 : 1;

                // Перезаполнение категории под текущий тип
                UpdateCategories();

                // Устанавливление категории
                cmbCategory.Text = selectedTransaction.Category;

                txtAmount.Text = selectedTransaction.Amount.ToString();
                txtDescription.Text = selectedTransaction.Description;
            }
        }

        // Тип из COMBOBOX 
        private string GetSelectedType()
        {
            if (cmbType.SelectedItem is ComboBoxItem item)
                return item.Content.ToString();
            return "Расход";
        }
    }
}