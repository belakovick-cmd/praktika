using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace z3
{
    // Класс книги
    public class Book
    {
        public int Id { get; set; }              // ID
        public string Title { get; set; }        // название
        public string Author { get; set; }       // автор
        public int Year { get; set; }            // год
        public string Genre { get; set; }        // жанр
        public string Renter { get; set; }       // арендатор
        public bool IsRented { get; set; }       // арендована?

        // Статус вычисляется — в файле не хранится
        public string Status => IsRented ? "Арендована" : "Доступна";
    }

    public partial class MainWindow : Window
    {
        // Все книги
        private ObservableCollection<Book> books = new ObservableCollection<Book>();

        // Отфильтрованные книги 
        private ObservableCollection<Book> filteredBooks = new ObservableCollection<Book>();

        // Выбранная книга
        private Book selectedBook = null;

        // Файл
        private const string FileName = "library.txt";
        private const char Separator = '|';

        // Конструктор
        public MainWindow()
        {
            InitializeComponent();

            // DataGrid привязан к отфильтрованному списку
            dgBooks.ItemsSource = filteredBooks;

            // Загрузка из файла
            LoadFromFile();

            // Если файла не было — добавляются примеры
            if (books.Count == 0)
            {
                books.Add(new Book { Id = 1, Title = "Война и мир", Author = "Л.Н. Толстой", Year = 1869, Genre = "Роман" });
                books.Add(new Book { Id = 2, Title = "Мастер и Маргарита", Author = "М.А. Булгаков", Year = 1967, Genre = "Роман" });
                books.Add(new Book { Id = 3, Title = "Преступление и наказание", Author = "Ф.М. Достоевский", Year = 1866, Genre = "Роман" });
                books.Add(new Book { Id = 4, Title = "1984", Author = "Дж. Оруэлл", Year = 1949, Genre = "Фантастика" });
                books.Add(new Book { Id = 5, Title = "Шерлок Холмс", Author = "А. Конан Дойл", Year = 1892, Genre = "Детектив" });
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

            foreach (var book in books)
            {
                // Экранируем | в полях на §
                string title = book.Title.Replace("|", "§");
                string author = book.Author.Replace("|", "§");
                string genre = book.Genre.Replace("|", "§");
                string renter = (book.Renter ?? "").Replace("|", "§");

                // Формат: Id|Название|Автор|Год|Жанр|Арендатор|Арендована
                string line = string.Join(Separator.ToString(),
                    book.Id,
                    title,
                    author,
                    book.Year,
                    genre,
                    renter,
                    book.IsRented ? "1" : "0");

                lines.Add(line);
            }

            File.WriteAllLines(FileName, lines, Encoding.UTF8);
        }

        // Загрузка из файла
        private void LoadFromFile()
        {
            books.Clear();

            if (!File.Exists(FileName)) return;

            string[] lines = File.ReadAllLines(FileName, Encoding.UTF8);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(Separator);
                if (parts.Length != 7) continue;

                try
                {
                    books.Add(new Book
                    {
                        Id = int.Parse(parts[0]),
                        Title = parts[1].Replace("§", "|"),
                        Author = parts[2].Replace("§", "|"),
                        Year = int.Parse(parts[3]),
                        Genre = parts[4].Replace("§", "|"),
                        Renter = parts[5].Replace("§", "|"),
                        IsRented = parts[6] == "1"
                    });
                }
                catch { /* битую строку пропускаем */ }
            }
        }

        // Следующий ID
        private int GetNextId()
        {
            if (books.Count == 0) return 1;
            return books.Max(b => b.Id) + 1;
        }

        // Фильтр (поиск)
        private void ApplyFilter()
        {
            string search = txtSearch.Text.Trim().ToLower();
            filteredBooks.Clear();

            foreach (var book in books)
            {
                if (string.IsNullOrEmpty(search) ||
                    book.Title.ToLower().Contains(search) ||
                    book.Author.ToLower().Contains(search))
                {
                    filteredBooks.Add(book);
                }
            }
        }

        // Поиск
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }

        // Кнопка "Добавить"
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название книги!", "Ошибка");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Введите автора!", "Ошибка");
                return;
            }

            if (!int.TryParse(txtYear.Text, out int year) ||
                year < 1000 || year > DateTime.Now.Year)
            {
                MessageBox.Show($"Введите корректный год (1000–{DateTime.Now.Year})!", "Ошибка");
                return;
            }

            books.Add(new Book
            {
                Id = GetNextId(),
                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                Year = year,
                Genre = GetSelectedGenre(),
                Renter = "",
                IsRented = false
            });

            SaveToFile();
            ApplyFilter();
            ClearForm();
        }

        // Кнопка "Арендовать"
        private void btnRent_Click(object sender, RoutedEventArgs e)
        {
            // Проверка: выбрана ли книга
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу в таблице!", "Ошибка");
                return;
            }

            // Проверка: не арендована ли уже
            if (selectedBook.IsRented)
            {
                MessageBox.Show($"Книга уже арендована: {selectedBook.Renter}", "Ошибка");
                return;
            }

            // Запрос на ввод имени арендатора
            string renter = PromptForRenter();
            if (string.IsNullOrWhiteSpace(renter)) return;

            // Сохранение названия до ApplyFilter()
            string bookTitle = selectedBook.Title;

            // Оформление аренды
            selectedBook.IsRented = true;
            selectedBook.Renter = renter.Trim();

            SaveToFile();
            ApplyFilter();   

            // Обновление статуса в форме
            lblStatus.Text = $"Арендована ({renter})";
            lblStatus.Foreground = System.Windows.Media.Brushes.Red;

            // Исполльзование локальной переменной
            MessageBox.Show($"Книга \"{bookTitle}\" выдана: {renter}");
        }

        // Кнопка "Вернуть"
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу в таблице!", "Ошибка");
                return;
            }

            if (!selectedBook.IsRented)
            {
                MessageBox.Show("Эта книга не арендована!", "Ошибка");
                return;
            }

            // Сохранение данных до ApplyFilter()
            string bookTitle = selectedBook.Title;
            string prevRenter = selectedBook.Renter;

            selectedBook.IsRented = false;
            selectedBook.Renter = "";

            SaveToFile();
            ApplyFilter();

            lblStatus.Text = "Доступна";
            lblStatus.Foreground = System.Windows.Media.Brushes.Green;

            MessageBox.Show($"Книга \"{bookTitle}\" возвращена от {prevRenter}");
        }

        // Кнопка "Удалить"
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу в таблице!", "Ошибка");
                return;
            }

            // Сохранение названия до удаления
            string bookTitle = selectedBook.Title;

            var result = MessageBox.Show(
                $"Удалить книгу \"{bookTitle}\"?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                books.Remove(selectedBook);
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
            txtTitle.Clear();
            txtAuthor.Clear();
            txtYear.Clear();
            cmbGenre.SelectedIndex = 0;

            selectedBook = null;
            dgBooks.SelectedItem = null;

            lblStatus.Text = "—";
            lblStatus.Foreground = System.Windows.Media.Brushes.Gray;
        }

        // Выбор книги в таблице
        private void dgBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBooks.SelectedItem == null) return;

            selectedBook = dgBooks.SelectedItem as Book;

            if (selectedBook != null)
            {
                txtTitle.Text = selectedBook.Title;
                txtAuthor.Text = selectedBook.Author;
                txtYear.Text = selectedBook.Year.ToString();
                cmbGenre.SelectedIndex = GenreToIndex(selectedBook.Genre);

                if (selectedBook.IsRented)
                {
                    lblStatus.Text = $"Арендована ({selectedBook.Renter})";
                    lblStatus.Foreground = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    lblStatus.Text = "Доступна";
                    lblStatus.Foreground = System.Windows.Media.Brushes.Green;
                }
            }
        }

        // Жанр из COMBOBOX 
        private string GetSelectedGenre()
        {
            if (cmbGenre.SelectedItem is ComboBoxItem item)
                return item.Content.ToString();
            return "Другое";
        }

        // Жанр -> индекс
        private int GenreToIndex(string genre)
        {
            switch (genre)
            {
                case "Роман": return 0;
                case "Фантастика": return 1;
                case "Детектив": return 2;
                case "Поэзия": return 3;
                case "Научная": return 4;
                default: return 5;
            }
        }

        // Окно ввода имени арендатора
        private string PromptForRenter()
        {
            Window dialog = new Window
            {
                Title = "Аренда книги",
                Width = 350,
                Height = 160,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(15) };

            panel.Children.Add(new TextBlock
            {
                Text = "Введите имя арендатора:",
                Margin = new Thickness(0, 0, 0, 5)
            });

            TextBox input = new TextBox
            {
                Margin = new Thickness(0, 0, 0, 10),
                Padding = new Thickness(3)
            };
            panel.Children.Add(input);

            string result = null;

            StackPanel buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Button okBtn = new Button
            {
                Content = "OK",
                Width = 80,
                Height = 30,
                Margin = new Thickness(5)
            };
            okBtn.Click += (s, e) =>
            {
                result = input.Text;
                dialog.Close();
            };

            Button cancelBtn = new Button
            {
                Content = "Отмена",
                Width = 80,
                Height = 30,
                Margin = new Thickness(5)
            };
            cancelBtn.Click += (s, e) => dialog.Close();

            buttons.Children.Add(okBtn);
            buttons.Children.Add(cancelBtn);
            panel.Children.Add(buttons);

            dialog.Content = panel;
            input.Focus();
            dialog.ShowDialog();

            return result;
        }
    }
}