using System;
using System.IO;

class FolderCleaner
{
    static void Main(string[] args)
    {
        string folderPath = "C:\\Users\\Diana\\Desktop\\Новая";

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Папка по пути \"{0}\" не существует.", folderPath);
            return;
        }

        TimeSpan timeSpan = TimeSpan.FromMinutes(30);

        try
        {
            long sizeBefore = GetDirectorySize(folderPath);
            Console.WriteLine("Размер папки до очистки: {0} байт.", sizeBefore);

            int deletedFilesCount = 0;
            long freedBytes = 0;

            CleanDirectory(folderPath, timeSpan, ref deletedFilesCount, ref freedBytes);

            Console.WriteLine("Очистка завершена.");
            Console.WriteLine("Удалено файлов: {0}", deletedFilesCount);
            Console.WriteLine("Освобождено места: {0} байт.", freedBytes);

            long sizeAfter = GetDirectorySize(folderPath);
            Console.WriteLine("Размер папки после очистки: {0} байт.", sizeAfter);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: {0}", ex.Message);
        }

    }

    static void CleanDirectory(string path, TimeSpan timeSpan, ref int deletedFilesCount, ref long freedBytes)
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
                    FileInfo fi = new FileInfo(file);
                    long fileSize = fi.Length;

                    File.Delete(file);
                    deletedFilesCount++;
                    freedBytes += fileSize;

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
                    long dirSize = GetDirectorySize(directory);
                    Directory.Delete(directory, true);
                    deletedFilesCount++;
                    freedBytes += dirSize;

                    Console.WriteLine("Удалена папка: {0}", directory);
                }
                else
                {
                    CleanDirectory(directory, timeSpan, ref deletedFilesCount, ref freedBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось удалить папку \"{0}\": {1}", directory, ex.Message);
            }
        }
    }

    public static long GetDirectorySize(string path)
    {
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException("Директория '" + path + "' не существует.");
        }

        long totalSize = 0;

        try
        {
           
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                try
                {
                    FileInfo fi = new FileInfo(file);
                    totalSize += fi.Length;
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Нет доступа к файлу '{0}'. Пропускаем.", file);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Файл '{0}' не найден. Возможно, он был удалён. Пропускаем.", file);
                }
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                try
                {
                    totalSize += GetDirectorySize(directory);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Нет доступа к директории '{0}'. Пропускаем.", directory);
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine("Директория '{0}' не найдена. Возможно, она была удалена. Пропускаем.", directory);
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет доступа к директории '{0}'.", path);
            throw;
        }
        catch (PathTooLongException)
        {
            Console.WriteLine("Путь к директории '{0}' слишком длинный.", path);
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка при обработке директории '{0}': {1}", path, ex.Message);
            throw;
        }

        return totalSize;
    }

}
