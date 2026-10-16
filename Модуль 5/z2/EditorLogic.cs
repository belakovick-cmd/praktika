using System.IO;       
using System.Text;     

namespace z2
{
    // Работа с файлами
    public static class EditorLogic
    {
        // Кнопка "Открыть файл"
        public static string OpenFile(string filePath)
        {
            // Чтение файла в кодировке UTF-8
            return File.ReadAllText(filePath, Encoding.UTF8);
        }

        // Кнопка "Сохранить файл"
        public static bool SaveFile(string filePath, string content)
        {
            // Запись текста в файл в кодировке UTF-8
            File.WriteAllText(filePath, content, Encoding.UTF8);
            return true;
        }
    }
}