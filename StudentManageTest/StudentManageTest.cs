using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentCalculations;


namespace StudentCalculations.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void TestCalculateSubjectMarks_PassCase()
        {
            // Arrange
            var subjectMarks = new Dictionary<string, int>
            {
                { "Subject1", 80 },
                { "Subject2", 90 },
                { "Subject3", 85 }
            };

            // Act
            var result = Program.CalculateSubjectMarks(subjectMarks);

            // Assert
            Assert.IsTrue(result.IsPass);
            Assert.AreEqual(255, result.TotalMarks);
            Assert.AreEqual(85, result.Percentage);
        }

        [TestMethod]
        public void TestCalculateSubjectMarks_FailCase()
        {
            // Arrange
            var subjectMarks = new Dictionary<string, int>
            {
                { "Subject1", 30 },
                { "Subject2", 40 },
                { "Subject3", 50 }
            };

            // Act
            var result = Program.CalculateSubjectMarks(subjectMarks);

            // Assert
            Assert.IsFalse(result.IsPass);
            Assert.AreEqual(120, result.TotalMarks);
            Assert.AreEqual(40, result.Percentage);
        }

        [TestMethod]
        public void TestGetStudentDetails_ReturnsDetails()
        {
            // Arrange
            var students = new List<(int, string, Dictionary<string, int>)>
            {
                (11, "Smita", new Dictionary<string, int>
                {
                    { "Subject1", 80 },
                    { "Subject2", 90 },
                    { "Subject3", 85 }
                }),
                (21, "Tara", new Dictionary<string, int>
                {
                    { "Subject1", 60 },
                    { "Subject2", 50 },
                    { "Subject3", 55 }
                })
            };

            // Act
            var details = Program.GetStudentDetails(students);

            // Assert
            Assert.AreEqual(2, details.Count);
            Assert.IsTrue(details[0].Contains("Smita"));
            Assert.IsTrue(details[1].Contains("Tara"));
        }

        [TestMethod]
        public void TestUpdateStudentMarks_ValidUpdate()
        {
            // Arrange
            var students = new List<(int, string, Dictionary<string, int> SubjectMarks)>
            {
                (1, "Smita", new Dictionary<string, int>
                {
                    { "Subject1", 80 },
                    { "Subject2", 90 },
                    { "Subject3", 85 }
                })
            };

            // Act
            var result = Program.UpdateStudentMarks(students, 1, "Subject1", 95);

            // Assert
            Assert.IsTrue(result.IsUpdated);
            Assert.AreEqual("Marks updated successfully.", result.Message);
            Assert.AreEqual(95, students[0].SubjectMarks["Subject1"]);
        }

        [TestMethod]
        public void TestUpdateStudentMarks_InvalidUpdate()
        {
            // Arrange
            var students = new List<(int, string, Dictionary<string, int>)>
            {
                (1, "Shreya", new Dictionary<string, int>
                {
                    { "Subject1", 80 },
                    { "Subject2", 90 },
                    { "Subject3", 85 }
                })
            };

            // Act
            var result = Program.UpdateStudentMarks(students, 2, "Subject4", 95);

            // Assert
            Assert.IsFalse(result.IsUpdated);
            Assert.AreEqual("Student or subject not found.", result.Message);
        }
    }
}
