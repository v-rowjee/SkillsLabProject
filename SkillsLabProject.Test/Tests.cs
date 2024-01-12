using Moq;
using SkillsLabProject.BL.BL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.Test
{
    [TestFixture]
    public class Tests
    {
        private Mock<IAppUserDAL> _stubAppUser;
        private Mock<IEmployeeDAL> _stubEmployee;
        private AppUserBL _appUserBL;
        List<AppUserModel> _appUsers;

        [SetUp]
        public void Setup()
        {
            _appUsers = new List<AppUserModel>()
            {
                new AppUserModel()
                {
                    AppUserId = 1,
                    Email = "user@ceridian.com",
                    Password = "AQAAAAEAACcQAAAAENygCewQKrMCm6H5EZA8tl46Si9JRNa6y/NjIgepbLPtYX2WlwuvsQed5Df17edZog==",
                    EmployeeId = 1
                }
            };
            _stubAppUser = new Mock<IAppUserDAL>();

            _stubAppUser
                .Setup(a => a.GetHashedPasswordAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync((LoginViewModel model) => _appUsers.Where(a => a.Email == model.Email).Select(a => a.Password).FirstOrDefault());

            _stubAppUser
                .Setup(a => a.RegisterUserAsync(It.IsAny<RegisterViewModel>()))
                .ReturnsAsync(true);

            List<EmployeeModel> employees = new List<EmployeeModel>()
            {
                new EmployeeModel()
                {
                    EmployeeId = 1,
                    FirstName = "User",
                    LastName = "Test",
                    NIC = "U0930029930492",
                    PhoneNumber = "57939928",
                    Email = "user@ceridian.com",
                    Department = new DepartmentModel()
                    {
                        DepartmentId = 1,
                        Title = "TestDepartment"
                    }
                }
            };
            _stubEmployee = new Mock<IEmployeeDAL>();
            _stubEmployee
                .Setup(e => e.GetAllEmployeesAsync())
                .ReturnsAsync(employees);

            _appUserBL = new AppUserBL(_stubAppUser.Object, _stubEmployee.Object);
        }

        [Test]
        [TestCase("user@ceridian.com", "4321", ExpectedResult = true)]
        [TestCase("user@ceridian.com", "0000", ExpectedResult = false)]
        [TestCase("notuser@ceridian.com", "4321", ExpectedResult = false)]
        public async Task<bool> Test_Login(string email, string password)
        {
            // Arrange
            var model = new LoginViewModel()
            {
                Email = email,
                Password = password,
            };
            // Act
            var result = await _appUserBL.AuthenticateUserAsync(model);
            // Assert
            return result.IsSuccess;
        }


        [Test]
        [TestCase("ved", "rowjee", 1, "ved.rowjee@ceridian.com", "1234", "R080901299930", "59749958", Role.Employee, ExpectedResult = "Success" )]
        [TestCase("ved", "rowjee", 1, "user@ceridian.com", "1234", "R080901299930", "59749958", Role.Employee, ExpectedResult = "DuplicatedEmail")]
        [TestCase("ved", "rowjee", 1, "ved.rowjee@ceridian.com", "1234", "U0930029930492", "59749958", Role.Employee, ExpectedResult = "DuplicatedNIC")]
        [TestCase("ved", "rowjee", 1, "user@ceridian.com", "1234", "U0930029930492", "59749958", Role.Employee, ExpectedResult = "DuplicatedEmail,DuplicatedNIC")]

        public async Task<string> Test_Register(string firstName, string lastName, int departmentId, string email, string password, string nic, string phoneNumber, Role role)
        {
            // Arrange
            var model = new RegisterViewModel()
            {
                FirstName = firstName,
                LastName = lastName,
                DepartmentId = departmentId,
                Email = email,
                Password = password,
                NIC = nic,
                PhoneNumber = phoneNumber,
                Role = role
            };

            // Act
            var result = await _appUserBL.RegisterUserAsync(model);

            // Assert
            return string.Join(",",result);
        }
    }
}