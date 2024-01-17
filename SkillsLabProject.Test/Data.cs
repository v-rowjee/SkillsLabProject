using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;

namespace SkillsLabProject.Test
{
    public class Data
    {
        public static List<AppUserModel> AppUsers = new List<AppUserModel>()
        {
            new AppUserModel() { AppUserId = 1, Email = "john.doe@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 1 },
            new AppUserModel() { AppUserId = 2, Email = "jane.smith@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 2 },
            new AppUserModel() { AppUserId = 3, Email = "michael.johnson@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 3 },
            new AppUserModel() { AppUserId = 4, Email = "emily.williams@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 4 },
            new AppUserModel() { AppUserId = 5, Email = "daniel.jones@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 5 },
            new AppUserModel() { AppUserId = 6, Email = "olivia.brown@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 6 },
            new AppUserModel() { AppUserId = 7, Email = "william.miller@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 7 },
            new AppUserModel() { AppUserId = 8, Email = "sophia.davis@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 8 },
            new AppUserModel() { AppUserId = 9, Email = "james.garcia@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 9 },
            new AppUserModel() { AppUserId = 10, Email = "ava.rodriguez@example.com", Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==", EmployeeId = 10 }
        };

        public static List<DepartmentModel> Departments = new List<DepartmentModel>()
        {
            new DepartmentModel(){ DepartmentId = 1, Title = "P&T" },
            new DepartmentModel(){ DepartmentId = 2, Title = "Support" },
        };


        public static List<EmployeeModel> Employees = new List<EmployeeModel>()
        {
            new EmployeeModel() { EmployeeId = 1, FirstName = "John", LastName = "Doe", NIC = "U1234567890123", PhoneNumber = "5551234", Email = "john.doe@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 2, FirstName = "Jane", LastName = "Smith", NIC = "U2345678901234", PhoneNumber = "5555678", Email = "jane.smith@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 3, FirstName = "Michael", LastName = "Johnson", NIC = "U3456789012345", PhoneNumber = "5559101", Email = "michael.johnson@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 4, FirstName = "Emily", LastName = "Williams", NIC = "U4567890123456", PhoneNumber = "5551122", Email = "emily.williams@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 5, FirstName = "Daniel", LastName = "Jones", NIC = "U5678901234567", PhoneNumber = "5553344", Email = "daniel.jones@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 6, FirstName = "Olivia", LastName = "Brown", NIC = "U6789012345678", PhoneNumber = "5555566", Email = "olivia.brown@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 7, FirstName = "William", LastName = "Miller", NIC = "U7890123456789", PhoneNumber = "5557788", Email = "william.miller@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 8, FirstName = "Sophia", LastName = "Davis", NIC = "U8901234567890", PhoneNumber = "5559900", Email = "sophia.davis@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 9, FirstName = "James", LastName = "Garcia", NIC = "U9012345678901", PhoneNumber = "5551122", Email = "james.garcia@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "P&T" } },
            new EmployeeModel() { EmployeeId = 10, FirstName = "Ava", LastName = "Rodriguez", NIC = "U0123456789012", PhoneNumber = "5553344", Email = "ava.rodriguez@example.com", Department = new DepartmentModel() { DepartmentId = 1, Title = "Support" } }
        };


        public static List<TrainingModel> Trainings = new List<TrainingModel>()
        {
            new TrainingModel() { TrainingId = 1, Title = "Introduction to Programming", Description = "Learn the basics of programming.", Deadline = DateTime.Now.AddMonths(2), Capacity = 50, PriorityDepartment = new DepartmentModel() { DepartmentId = 101, Title = "P&T" }, IsClosed = false },
            new TrainingModel() { TrainingId = 2, Title = "Web Development Fundamentals", Description = "Basic concepts of web development.", Deadline = DateTime.Now.AddMonths(2), Capacity = 40, PriorityDepartment = new DepartmentModel() { DepartmentId = 101, Title = "P&T" }, IsClosed = false },
            new TrainingModel() { TrainingId = 3, Title = "Database Management", Description = "Introduction to database systems.", Deadline = DateTime.Now.AddMonths(2), Capacity = 35, PriorityDepartment = new DepartmentModel() { DepartmentId = 101, Title = "P&T" }, IsClosed = false },
            new TrainingModel() { TrainingId = 4, Title = "Software Engineering Principles", Description = "Fundamentals of software engineering.", Deadline = DateTime.Now.AddMonths(2), Capacity = 45, PriorityDepartment = new DepartmentModel() { DepartmentId = 101, Title = "P&T" }, IsClosed = false },
            new TrainingModel() { TrainingId = 5, Title = "Algorithms and Data Structures", Description = "Essential algorithms and data structures.", Deadline = DateTime.Now.AddMonths(2), Capacity = 30, PriorityDepartment = new DepartmentModel() { DepartmentId = 101, Title = "P&T" }, IsClosed = false },
            new TrainingModel() { TrainingId = 6, Title = "Network Security Basics", Description = "Introduction to network security.", Deadline = DateTime.Now.AddMonths(2), Capacity = 25, PriorityDepartment = new DepartmentModel() { DepartmentId = 101, Title = "P&T" }, IsClosed = false },
            new TrainingModel() { TrainingId = 7, Title = "Data Science Essentials", Description = "Fundamental concepts in data science.", Deadline = DateTime.Now.AddMonths(3), Capacity = 30, PriorityDepartment = new DepartmentModel() { DepartmentId = 201, Title = "Support" }, IsClosed = false }
        };


        public static List<EnrollmentModel> Enrollments = new List<EnrollmentModel>()
        {
            new EnrollmentModel() { EnrollmentId = 1, EmployeeId = 1, TrainingId = 1, Status = Status.Approved, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-2), CreatedOn = DateTime.Now.AddHours(-5) },
            new EnrollmentModel() { EnrollmentId = 2, EmployeeId = 2, TrainingId = 1, Status = Status.Pending, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-1), CreatedOn = DateTime.Now.AddHours(-4) },
            new EnrollmentModel() { EnrollmentId = 3, EmployeeId = 3, TrainingId = 1, Status = Status.Declined, DeclinedReason = "Not available on scheduled dates", UpdatedOn = DateTime.Now.AddHours(1), CreatedOn = DateTime.Now.AddHours(-3) },
            new EnrollmentModel() { EnrollmentId = 4, EmployeeId = 4, TrainingId = 1, Status = Status.Approved, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-1), CreatedOn = DateTime.Now.AddHours(-2) },
            new EnrollmentModel() { EnrollmentId = 5, EmployeeId = 5, TrainingId = 1, Status = Status.Pending, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-3), CreatedOn = DateTime.Now.AddHours(-6) },
            new EnrollmentModel() { EnrollmentId = 6, EmployeeId = 6, TrainingId = 1, Status = Status.Approved, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-2), CreatedOn = DateTime.Now.AddHours(-4) },
            new EnrollmentModel() { EnrollmentId = 7, EmployeeId = 7, TrainingId = 1, Status = Status.Approved, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-1), CreatedOn = DateTime.Now.AddHours(-3) },
            new EnrollmentModel() { EnrollmentId = 8, EmployeeId = 7, TrainingId = 2, Status = Status.Pending, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-2), CreatedOn = DateTime.Now.AddHours(-5) },
            new EnrollmentModel() { EnrollmentId = 9, EmployeeId = 8, TrainingId = 2, Status = Status.Declined, DeclinedReason = "Unavailable during training period", UpdatedOn = DateTime.Now.AddHours(2), CreatedOn = DateTime.Now.AddHours(-1) },
            new EnrollmentModel() { EnrollmentId = 10, EmployeeId = 9, TrainingId = 3, Status = Status.Approved, DeclinedReason = null, UpdatedOn = DateTime.Now.AddHours(-1), CreatedOn = DateTime.Now.AddHours(-3) }
        };

    }
}
