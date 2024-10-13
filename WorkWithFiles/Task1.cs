using System;
using System.IO;

class FolderCleaner
{
    static void Main(string[] args)
    {
        string folderPath = "C:\\Users\\Diana\\Desktop\\Новая";

        // Проверка, существует ли папка
        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine($"Папка по пути \"{folderPath}\" не существует.");
            return;
        }
        
        TimeSpan timeSpan = TimeSpan.FromMinutes(30);

        try
        {
            CleanDirectory(folderPath, timeSpan);
            Console.WriteLine("Очистка завершена.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    
    }

    static void CleanDirectory(string path, TimeSpan timeSpan)
    {       
        DateTime now = DateTime.Now;

        // Удаление файлов
        foreach (var file in Directory.GetFiles(path))
        {
            try
            {
                DateTime lastAccess = File.GetLastAccessTime(file);
                if (now - lastAccess > timeSpan)
                {
                    File.Delete(file);
                    Console.WriteLine("Удалён файл: {0}", file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось удалить файл \"{0}\": {1}", file, ex.Message);
            }
        }

        // Удаление папок
        foreach (var directory in Directory.GetDirectories(path))
        {
            try
            {
                DateTime lastAccess = Directory.GetLastAccessTime(directory);
                if (now - lastAccess > timeSpan)
                {
                    Directory.Delete(directory, true);
                    Console.WriteLine("Удалён файл: {0}", directory);
                }
                else
                {                    
                    CleanDirectory(directory, timeSpan);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось удалить папку \"{0}\": {1}", directory, ex.Message);
            }
        }
    }
}
