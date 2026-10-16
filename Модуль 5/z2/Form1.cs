using System;
using System.Windows.Forms;

namespace z2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Кнопка "Открыть"
        private void btnOpen_Click(object sender, EventArgs e)
        {
            // Диалог выбора файла
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Вызов метода из другого файла
                    txtEditor.Text = EditorLogic.OpenFile(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Кнопка "Сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Диалог сохранения
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Вызов метода из другого файла
                    EditorLogic.SaveFile(saveFileDialog1.FileName, txtEditor.Text);

                    MessageBox.Show("Файл сохранён!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}