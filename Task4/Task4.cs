using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal AverageGrade { get; set; }

    public override string ToString()
    {
        return $"{Name}, {DateOfBirth.ToShortDateString()}, {AverageGrade:F2}";
    }
}

class Program
{
    static void Main()
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string studentsDirectory = Path.Combine(desktopPath, "Students");
        Directory.CreateDirectory(studentsDirectory);

        string binaryFilePath = "C:\\Users\\Diana\\Downloads\\BinaryReadWrite-64e19b2f1cdf6b54b8a760e20320e2a9ae4a6f04\\BinaryReadWrite-64e19b2f1cdf6b54b8a760e20320e2a9ae4a6f04\\BinaryReadWrite\\students.bin"; 

        List<Student> students = new List<Student>();
               
        using (FileStream fs = new FileStream(binaryFilePath, FileMode.Open, FileAccess.Read))
        using (BinaryReader reader = new BinaryReader(fs, Encoding.UTF8))
        {
            while (fs.Position < fs.Length)
            {
                string name = reader.ReadString();
                string group = reader.ReadString();
                long dateBinary = reader.ReadInt64();
                DateTime dateOfBirth = DateTime.FromBinary(dateBinary);
                decimal averageGrade = reader.ReadDecimal();

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
               
        foreach (var student in students)
        {
            string groupFilePath = Path.Combine(studentsDirectory, $"{student.Group}.txt");
                        
            using (StreamWriter writer = new StreamWriter(groupFilePath, append: true))
            {
                writer.WriteLine(student.ToString());
            }
        }

        Console.WriteLine("Загрузка завершена. Файлы со студентами созданы на рабочем столе.");
    }
}
