using System;
using System.Collections.Generic;

namespace Student_Calculations
{
    internal class Program
    {
        //Function to Print Each Subject Marks from Dictionary
        static void Subject_Marks(Dictionary<string, int> All_Subject_Marks) 
        {
            Boolean pass = true;
            int Total = 0;
            foreach (var Subject_Marks in All_Subject_Marks)
            {
                if (Subject_Marks.Value < 35)
                {
                    pass = false;
                }
                Console.Write($"{Subject_Marks.Key} : {Subject_Marks.Value}, ");
                Total += Subject_Marks.Value;
            }
            decimal Percentage = ((decimal)Total / 300) * 100;
            Console.WriteLine($"\nTotal Marks: {Total}, Percentage: {Percentage}\n");

            if(Percentage >= 35 && pass == true)
            {
                Console.WriteLine("Result: Pass");
            }
            else
            {
                Console.WriteLine("Result: Failed");
            }
        }

        //Function to Print All Students Details
        static void Display_Details(List<(int Student_ID, string Student_Name, Dictionary<string, int> Student_Marks)> Student_Info)
        {
            foreach (var Student in Student_Info)
            {
                Console.Write($"\nID: {Student.Student_ID}, Name: {Student.Student_Name}");
                Console.Write("\nSubject Marks: ");
                Subject_Marks(Student.Student_Marks);
            }
        }

        //Function to Create New Student Record
        static void Create_Record(List<(int Student_ID, string Student_Name, Dictionary<string, int> Student_Marks)> Student_Info)
        {
            Console.WriteLine("\nEnter number of Students: ");
            int Number_of_Student = int.Parse(Console.ReadLine()); //Total number of Student Detail Entries to take

            for (int i = 1; i <= Number_of_Student; i++)
            {
                //Enter Details of One Student at a time
                Console.WriteLine($"\nEnter Details for student {i}");

                //Specifying format for input values separation within one record
                Console.Write("\nEnter Student Details in following format:" +
                    "\nID, Name, Subject1_Marks, Subject2_Marks, Subject3_Marks\n\n");
                string input = Console.ReadLine();

                //Take input value separated by comma as one detail
                string[] details = input.Split(",");

                int ID = int.Parse(details[0].Trim());

                string Name = details[1].Trim();

                //Take marks of different subjects into single Dictionary named as Marks
                var Marks = new Dictionary<string, int>
                {
                    {"Subject1", int.Parse(details[2].Trim()) },
                    {"Subject2", int.Parse(details[3].Trim()) },
                    {"Subject3", int.Parse(details[4].Trim()) }
                };

                //Put one student detail as one list element in Student Info
                Student_Info.Add((ID, Name, Marks));
            }
        }

        //Function to Calculate Total & Percentage
        static void Calculate_Marks(List<(int Student_ID, string Student_Name, Dictionary<string, int> Student_Marks)> Student_Info)
        {
            Display_Details(Student_Info);
        }
        static void Main(string[] args)
        {
            List<(int Student_ID, string Student_Name, Dictionary<string, int> Student_Marks)> Student_Info = new List<(int, string, Dictionary<string, int>)>();

            Create_Record(Student_Info);

            int Max_Marks = 100; //Declare Maximum Marks
            int choice;
            do
            {
                Console.WriteLine("\nStudent Performance Management System");
                Console.WriteLine("1. View all students");
                Console.WriteLine("2. View specific student details by Student ID");
                Console.WriteLine("3. Create New Student Record");
                Console.WriteLine("4. Update student marks");
                Console.WriteLine("5. Calculate Total and Percentage");
                Console.WriteLine("6. Exit");
                Console.WriteLine("Enter your choice: ");
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                    {
                    case 1: //From the list of Students, Display Each Student's ID and Name
                        Display_Details(Student_Info);
                        break;
                    case 2:
                        string Search_Again = "y";

                        while (Search_Again == "y")
                        {
                            Console.WriteLine("\nEnter Student ID: ");
                            int Searched_ID = int.Parse(Console.ReadLine());


                            foreach (var Student in Student_Info)
                            { // Check for Searched StudentID in existing records
                                if (Student.Student_ID == Searched_ID)
                                {
                                    Console.WriteLine($"\nID: {Student.Student_ID}, Name: {Student.Student_Name}, Subject Marks: ");
                                    Subject_Marks(Student.Student_Marks); //Function to print each Dictionary Element
                                }
                                else
                                {
                                    Console.WriteLine("\nStudent Record not found\n");
                                }
                            }

                            Console.WriteLine("\nSearch Again? y/n");
                            Search_Again = Console.ReadLine();
                        }
                        break;
                    case 3: Create_Record(Student_Info);
                        break;
                    case 4: Console.WriteLine("\nEnter Student ID, Subject Name & Updated Marks: ");
                        string input = Console.ReadLine();
                        string[] update_input = input.Split(" ");

                        
                        int S_ID = int.Parse(update_input[0].Trim());
                        string Selected_Subject = update_input[1].Trim();
                        int Updated_Marks = int.Parse(update_input[2].Trim());


                            foreach (var Student in Student_Info)
                            {
                                if (Student.Student_ID == S_ID && Student.Student_Marks.ContainsKey(Selected_Subject))
                                {
                                    Student.Student_Marks[Selected_Subject] = Updated_Marks; // Using indexer to update value
                                }
                            }
                        Display_Details(Student_Info);
                        break;
                    case 5: Calculate_Marks(Student_Info);
                        break;
                    case 6: Console.WriteLine("\nExiting...");
                        break;

                }
            } while (choice != 4);
            }

    }
}