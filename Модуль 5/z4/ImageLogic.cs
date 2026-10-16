using System;                   
using System.Drawing;           
using System.IO;               

namespace z4
{
    // Логика работы с изображением
    public static class ImageLogic
    {
        // Загруженное изображение
        public static Image CurrentImage = null;

        // Текущий масштаб
        public static double Zoom = 1.0;

        // Шаг масштабирования
        public const double ZoomStep = 0.2;

        // Границы масштаба
        public const double MinZoom = 0.1;   // 10%
        public const double MaxZoom = 5.0;   // 500%

        // Кнопка "Открыть изображение"
        // Возвращение: true — успех, false — ошибка
        public static bool OpenImage(string filePath)
        {
            try
            {
                // Читение файла через FileStream — чтобы не блокировать
                using (FileStream fs = new FileStream(
                    filePath, FileMode.Open, FileAccess.Read))
                {
                    CurrentImage = Image.FromStream(fs);
                }

                Zoom = 1.0;   // сброс масштаба
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Кнопка "Увеличить"
        public static void ZoomIn()
        {
            if (CurrentImage == null) return;

            Zoom += ZoomStep;
            if (Zoom > MaxZoom) Zoom = MaxZoom;
        }

        // Кнопка "Уменьшить"
        public static void ZoomOut()
        {
            if (CurrentImage == null) return;

            Zoom -= ZoomStep;
            if (Zoom < MinZoom) Zoom = MinZoom;
        }

        // Кнопка "Сбросить на 100%"
        public static void ResetZoom()
        {
            if (CurrentImage == null) return;
            Zoom = 1.0;
        }

        // Кнопка "По размеру окна"
        public static void FitToSize(int containerWidth, int containerHeight)
        {
            if (CurrentImage == null) return;

            double widthRatio = (double)containerWidth / CurrentImage.Width;
            double heightRatio = (double)containerHeight / CurrentImage.Height;

            // Меньшее из двух — чтобы влезло целиком
            Zoom = Math.Min(widthRatio, heightRatio);
        }

        // Кнопка "Масштаб колёсиком"
        public static void ZoomByWheel(int delta)
        {
            if (CurrentImage == null) return;

            if (delta > 0) Zoom += ZoomStep;
            else Zoom -= ZoomStep;

            if (Zoom < MinZoom) Zoom = MinZoom;
            if (Zoom > MaxZoom) Zoom = MaxZoom;
        }

        // Получение масштабированного изображения
        // Возвращение нового изображения нужного размера
        public static Image GetScaledImage()
        {
            if (CurrentImage == null) return null;

            int newWidth = (int)(CurrentImage.Width * Zoom);
            int newHeight = (int)(CurrentImage.Height * Zoom);

            return new Bitmap(CurrentImage, newWidth, newHeight);
        }
    }
}