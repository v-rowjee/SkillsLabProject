using Moq;
using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.Test
{
    public class Tests
    {
        private Mock<IAppUserDAL> _stubAppUser;
        private Mock<IEmployeeDAL> _stubEmployee;
        private AppUserBL _appUserBL;

        [SetUp]
        public void Setup()
        {
            List<AppUserModel> users = new List<AppUserModel>()
            {
                new AppUserModel()
                {
                    AppUserId = 1,
                    Email = "user@ceridial.com",
                    Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==",
                    EmployeeId = 1
                }
            };
            _stubAppUser = new Mock<IAppUserDAL>();
            _stubAppUser
                .Setup(iappuserdal => iappuserdal.AuthenticateUserAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);


            List<EmployeeModel> employees = new List<EmployeeModel>()
            {
                new EmployeeModel()
                {
                    EmployeeId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    NIC = "Test",
                    PhoneNumber = "57939928",
                    Email = "user@ceridial.com",
                    Department = new DepartmentModel()
                    {
                        DepartmentId = 1,
                        Title = "TestDepartment"
                    },
                    Role = Role.Employee
                }
            };
            _stubEmployee = new Mock<IEmployeeDAL>();
            _stubEmployee
                .Setup(iEmployeeDAL => iEmployeeDAL.GetAllEmployeesAsync())
                .ReturnsAsync(employees);

            _appUserBL = new AppUserBL(_stubAppUser.Object,_stubEmployee.Object);
        }

        [Test]
        public void Test_Login()
        {
            
            Assert.Pass();
        }
    }
}