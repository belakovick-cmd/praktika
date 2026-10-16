using System;                   
using System.Windows.Forms;     

namespace z3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Кнопка "Добавить"
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Вызов метода из другого файла
            bool success = TaskLogic.AddTask(txtTask.Text);

            // Если не получилось — ошибка
            if (!success)
            {
                MessageBox.Show("Введите текст задачи!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Обновление списка в интерфейсе
            RefreshList();

            // Очищение полей и возвращение курсора
            txtTask.Clear();
            txtTask.Focus();
        }

        // Кнопка "Удалить"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Проверка: выбрана ли задача
            if (lstTasks.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Вызов метода из другого файла
            TaskLogic.DeleteTask(lstTasks.SelectedIndex);

            // Обновление списка
            RefreshList();
        }

        // Кнопка "Отметить выполненной"
        private void btnComplete_Click(object sender, EventArgs e)
        {
            // Проверка: выбрана ли задача
            if (lstTasks.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите задачу!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Вызов метода из другого файла
            TaskLogic.ToggleComplete(lstTasks.SelectedIndex);

            // Обнновление списка
            RefreshList();
        }

        // Обновление списка в интерфейсе
        // Копирование данных из TaskLogic в ListBox
        private void RefreshList()
        {
            // Запоминание выделения
            int selected = lstTasks.SelectedIndex;

            // Очистка ListBox
            lstTasks.Items.Clear();

            // Заполнение заново
            foreach (string task in TaskLogic.GetAll())
                lstTasks.Items.Add(task);

            // Восстанавление выделения (если возможно)
            if (selected >= 0 && selected < lstTasks.Items.Count)
                lstTasks.SelectedIndex = selected;
        }
    }
}