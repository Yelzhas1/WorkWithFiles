using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StudentDataLoader
{
    class Program
    {
        // Путь к бинарному файлу
        private const string BinaryFilePath = @"C:\Users\Diana\Desktop\Program";

        static void Main(string[] args)
        {
            try
            {
                // Считываем данные о студентах из бинарного файла
                List<Student> students = ReadStudentsFromBinaryFile(BinaryFilePath);

                if (students.Count == 0)
                {
                    Console.WriteLine("Файл не содержит данных о студентах.");
                    return;
                }

                // Создаем директорию Students на рабочем столе
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string studentsDirectory = Path.Combine(desktopPath, "Students");

                if (!Directory.Exists(studentsDirectory))
                {
                    Directory.CreateDirectory(studentsDirectory);
                }

                // Группируем студентов по группе
                var groupedStudents = students.GroupBy(s => s.Group);

                // Для каждой группы создаем отдельный текстовый файл
                foreach (var group in groupedStudents)
                {
                    string groupFilePath = Path.Combine(studentsDirectory, $"{group.Key}.txt");

                    using (StreamWriter writer = new StreamWriter(groupFilePath))
                    {
                        foreach (var student in group)
                        {
                            string line = $"{student.Name}, {student.DateOfBirth.ToString("dd.MM.yyyy")}, {student.AverageGrade}";
                            writer.WriteLine(line);
                        }
                    }
                }

                Console.WriteLine("Данные успешно загружены и распределены по группам.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл не найден по пути: {BinaryFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
                
        private static List<Student> ReadStudentsFromBinaryFile(string filePath)
        {
            List<Student> students = new List<Student>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    // Считываем имя
                    string name = reader.ReadString();

                    // Считываем группу
                    string group = reader.ReadString();

                    // Считываем дату рождения как long и преобразуем в DateTime
                    long dateBinary = reader.ReadInt64();
                    DateTime dateOfBirth = DateTime.FromBinary(dateBinary);

                    // Считываем средний балл
                    decimal averageGrade = reader.ReadDecimal();

                    // Создаем объект студента и добавляем в список
                    Student student = new Student
                    {
                        Name = name,
                        Group = group,
                        DateOfBirth = dateOfBirth,
                        AverageGrade = averageGrade
                    };

                    students.Add(student);
                }
            }

            return students;
        }
    }

    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AverageGrade { get; set; }
    }
}
