using System;
using System.Collections.Generic;
using NLog;
using Serilog;

namespace StudentCalculations
{
    internal class Program
    {   //Initialize NLog logger
        private static readonly Logger nLogger = LogManager.GetCurrentClassLogger();

        // Returns Result, Total Marks and Percentage
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

        // Returns Tuple containing Student Details (Without Separate Subject Marks)
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

        // Returns Student tuple for newly created record
        static List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)> CreateStudentRecords()
        {
            var students = new List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)>();

            Console.WriteLine("Enter the number of students: ");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int studentCount) || studentCount <= 0)
            {
                Log.Warning("Invalid input for student count. Exiting.");
                return students;
            }

            for (int i = 1; i <= studentCount; i++)
            {
                Console.WriteLine($"Enter details for student {i} (Format: ID, Name, Subject1_Marks, Subject2_Marks, Subject3_Marks):");
                string? studentInput = Console.ReadLine();

                if (studentInput is null)
                {
                    Log.Warning("Null input received. Skipping.");
                    continue;
                }

                string[] details = studentInput.Split(',');

                if (details.Length != 5 || !int.TryParse(details[0].Trim(), out int id))
                {
                    Log.Warning("Invalid input format for student details. Skipping.");
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

        // Returns Updated Marks Tuple for Existing Student
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
            // List for All Students Record
            var students = new List<(int StudentId, string StudentName, Dictionary<string, int> SubjectMarks)>();

            int choice;
            do
            {
                nLogger.Trace("Main Menu Opened.");

                Console.WriteLine("\nStudent Performance Management System");
                Console.WriteLine("1. View all students");
                Console.WriteLine("2. View specific student details");
                Console.WriteLine("3. Add new student records");
                Console.WriteLine("4. Update student marks");
                Console.WriteLine("5. Exit");
                Console.WriteLine("Enter your choice: ");

                string? input = Console.ReadLine();
                if (!int.TryParse(input, out choice))
                {
                    Log.Warning("Invalid choice input. Try again.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        nLogger.Trace("Viewed All Students.");
                        var allStudentDetails = GetStudentDetails(students);
                        foreach (var detail in allStudentDetails)
                        {
                            Console.WriteLine(detail);
                        }
                        break;

                    case 2:
                        Console.WriteLine("Enter Student ID:");
                        string? idInput = Console.ReadLine();
                        if (int.TryParse(idInput, out int studentId))
                        {
                            var student = students.Find(s => s.StudentId == studentId);
                            if (student.StudentId != 0)
                            {
                                nLogger.Trace($"Student ID {studentId} viewed.");
                                var result = CalculateSubjectMarks(student.SubjectMarks);
                                Console.WriteLine($"\nID: {student.StudentId}, Name: {student.StudentName}" +
                                                  $"\nTotal Marks: {result.TotalMarks}, Percentage: {result.Percentage:F2}%" +
                                                  $"\nResult: {(result.Percentage >= 35 && result.IsPass ? "Pass" : "Fail")}");
                            }
                            else
                            {
                                Log.Information($"Student with ID {studentId} not found.");
                            }
                            nLogger.Debug($"Student ID {studentId} not numeric.");
                        }
                        break;

                    case 3:
                        nLogger.Trace($"New Student Record Addition.");
                        students.AddRange(CreateStudentRecords());
                        break;

                    case 4:
                        Console.WriteLine("Enter Student ID, Subject Name, and Updated Marks (Format: ID Subject Marks):");
                        string? updateInput = Console.ReadLine();

                        if (updateInput is null)
                        {
                            Log.Warning("Null input for updating marks.");
                            continue;
                        }

                        string[] details = updateInput.Split(' ');

                        if (details.Length != 3 || !int.TryParse(details[0], out int id) || !int.TryParse(details[2], out int updatedMarks))
                        {
                            Log.Warning("Invalid input format for updating marks.");
                            continue;
                        }

                        nLogger.Debug($"Updation of Marks for {details[0]} initiated.");
                        string subject = details[1];
                        var updateResult = UpdateStudentMarks(students, id, subject, updatedMarks);
                        Log.Information(updateResult.Message);
                        break;

                    case 5:
                        Log.Information("Exiting application...");
                        break;

                    default:
                        Log.Warning("Invalid choice. Try again.");
                        break;
                }
            } while (choice != 5);
        }

        static void Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("C:/Users/Kavi/Desktop/Rite/Student_Calculations/logs/application-log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Application starting...");
                MainMenu();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred.");
            }
            finally
            {
                Log.Information("Application closing...");
                Log.CloseAndFlush();
            }
        }
    }
}
