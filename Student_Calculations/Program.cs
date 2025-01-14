using System;
using System.Collections.Generic;

namespace StudentCalculations
{
    internal class Program
    {
        static (bool IsPass, int TotalMarks, decimal Percentage) CalculateSubjectMarks(Dictionary<string, int> subjectMarks)
        {
            bool isPass = true;
            int totalMarks = 0;

            foreach (var subject in subjectMarks)
            {
                if (subject.Value < 35)
                {
                    isPass = false;
                }
                totalMarks += subject.Value;
            }

            decimal percentage = (decimal)totalMarks / 300 * 100;
            return (isPass, totalMarks, percentage);
        }

        static List<string> GetStudentDetails(List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)> students)
        {
            var studentDetails = new List<string>();

            foreach (var student in students)
            {
                var result = CalculateSubjectMarks(student.SubjectMarks);
                string details = $"\nID: {student.StudentId}, Name: {student.StudentName}" +
                                 $"\nTotal Marks: {result.TotalMarks}, Percentage: {result.Percentage:F2}%" +
                                 $"\nResult: {(result.Percentage >= 35 && result.IsPass ? "Pass" : "Fail")}";
                studentDetails.Add(details);
            }

            return studentDetails;
        }

        static List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)> CreateStudentRecords()
        {
            var students = new List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)>();

            Console.WriteLine("Enter the number of students: ");
            if (!int.TryParse(Console.ReadLine(), out int studentCount) || studentCount <= 0)
            {
                Console.WriteLine("Invalid input. Exiting.");
                return students;
            }

            for (int i = 1; i <= studentCount; i++)
            {
                Console.WriteLine($"Enter details for student {i} (Format: ID, Name, Subject1_Marks, Subject2_Marks, Subject3_Marks):");
                string input = Console.ReadLine();
                string[] details = input.Split(',');

                if (details.Length != 5 || !int.TryParse(details[0].Trim(), out int id))
                {
                    Console.WriteLine("Invalid input format. Skipping.");
                    continue;
                }

                string name = details[1].Trim();
                var marks = new Dictionary<string, int>
                {
                    { "Subject1", int.Parse(details[2].Trim()) },
                    { "Subject2", int.Parse(details[3].Trim()) },
                    { "Subject3", int.Parse(details[4].Trim()) }
                };

                students.Add((id, name, marks));
            }

            return students;
        }

        static (bool IsUpdated, string Message) UpdateStudentMarks(
            List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)> students, int studentId, string subject, int newMarks)
        {
            foreach (var student in students)
            {
                if (student.StudentId == studentId && student.SubjectMarks.ContainsKey(subject))
                {
                    student.SubjectMarks[subject] = newMarks;
                    return (true, "Marks updated successfully.");
                }
            }
            return (false, "Student or subject not found.");
        }

        static void MainMenu()
        {
            var students = new List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)>();

            int choice;
            do
            {
                Console.WriteLine("\nStudent Performance Management System");
                Console.WriteLine("1. View all students");
                Console.WriteLine("2. View specific student details");
                Console.WriteLine("3. Add new student records");
                Console.WriteLine("4. Update student marks");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        var allStudentDetails = GetStudentDetails(students);
                        foreach (var detail in allStudentDetails)
                        {
                            Console.WriteLine(detail);
                        }
                        break;

                    case 2:
                        Console.WriteLine("Enter Student ID:");
                        if (int.TryParse(Console.ReadLine(), out int studentId))
                        {
                            var student = students.Find(s => s.StudentId == studentId);
                            if (student.StudentId != 0)
                            {
                                var result = CalculateSubjectMarks(student.SubjectMarks);
                                Console.WriteLine($"\nID: {student.StudentId}, Name: {student.StudentName}" +
                                                  $"\nTotal Marks: {result.TotalMarks}, Percentage: {result.Percentage:F2}%" +
                                                  $"\nResult: {(result.Percentage >= 35 && result.IsPass ? "Pass" : "Fail")}");
                            }
                            else
                            {
                                Console.WriteLine("Student not found.");
                            }
                        }
                        break;

                    case 3:
                        students.AddRange(CreateStudentRecords());
                        break;

                    case 4:
                        Console.WriteLine("Enter Student ID, Subject Name, and Updated Marks (Format: ID Subject Marks):");
                        string input = Console.ReadLine();
                        string[] details = input.Split(' ');

                        if (details.Length != 3 || !int.TryParse(details[0], out studentId) || !int.TryParse(details[2], out int updatedMarks))
                        {
                            Console.WriteLine("Invalid input format.");
                            continue;
                        }

                        string subject = details[1];
                        var updateResult = UpdateStudentMarks(students, studentId, subject, updatedMarks);
                        Console.WriteLine(updateResult.Message);
                        break;

                    case 5:
                        Console.WriteLine("Exiting...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            } while (choice != 5);
        }

        static void Main(string[] args)
        {
            MainMenu();
        }
    }
}
