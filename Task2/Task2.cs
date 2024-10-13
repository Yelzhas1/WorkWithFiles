using System;
using System.IO;

class Program
{   
    public static long GetDirectorySize(string path)
    {        
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"Директория '{path}' не существует.");
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
                    Console.WriteLine("Файл '{0}' не найден. Возможно, он был удален. Пропускаем.", file);
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

    static void Main(string[] args)
    {
        string directoryPath = "C:\\Users\\Diana\\Desktop\\Новая";

        try
        {
            long sizeInBytes = GetDirectorySize(directoryPath);
            Console.WriteLine("Размер директории '{0}': {1} байт.", directoryPath, sizeInBytes);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Ошибка: {0}", ex.Message);
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine("Ошибка: {0}", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine("Ошибка: Нет доступа к одной из директорий или файлов. {0}", ex.Message);
        }
        catch (PathTooLongException ex)
        {
            Console.WriteLine("Ошибка: {0}", ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла неожиданная ошибка: {0}", ex.Message);
        }
    }
}
