using System;                   
using System.Drawing;           
using System.Windows.Forms;     

namespace z4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Двойная буферизация 
            typeof(PictureBox)
                .GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance)
                .SetValue(picImage, true, null);

            // Масштаб колёсиком мыши
            picImage.MouseWheel += PicImage_MouseWheel;
        }

        // Обновление изображения в PictureBox
        // Получает масштабированное изображение из ImageLogic
        private void RefreshImage()
        {
            Image scaled = ImageLogic.GetScaledImage();

            if (scaled != null)
                picImage.Image = scaled;
        }

        // Кнопка "Открыть"
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Вызов метода из другого файла
                bool success = ImageLogic.OpenImage(openFileDialog1.FileName);

                if (success)
                {
                    RefreshImage();
                }
                else
                {
                    MessageBox.Show("Не удалось открыть файл!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Кнопка "Увеличить"
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            // Вызов метода из другого файла
            ImageLogic.ZoomIn();
            RefreshImage();
        }

        // Кнопка "Увеличить"
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            // Вызов метода из другого файла
            ImageLogic.ZoomOut();
            RefreshImage();
        }

        // Кнопка "100"
        private void btnActual_Click(object sender, EventArgs e)
        {
            // Вызов метода из другого файла
            ImageLogic.ResetZoom();
            RefreshImage();
        }

        // Кнопка "По размеру окна"
        private void btnFit_Click(object sender, EventArgs e)
        {
            // Вызов метода из другого файла
            ImageLogic.FitToSize(picImage.Width, picImage.Height);
            RefreshImage();
        }

        // Колёсико мыши
        private void PicImage_MouseWheel(object sender, MouseEventArgs e)
        {
            // Вызов метода из другого файла
            ImageLogic.ZoomByWheel(e.Delta);
            RefreshImage();
        }
    }
}