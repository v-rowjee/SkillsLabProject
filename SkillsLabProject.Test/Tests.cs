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
        private Mock<IDepartmentDAL> _stubDepartment;
        private Mock<IEmployeeDAL> _stubEmployee;
        private Mock<IEnrollmentDAL> _stubEnrollment;
        private Mock<IPreRequisiteDAL> _stubPrerequisite;
        private Mock<ITrainingDAL> _stubTraining;

        private AppUserBL _appUserBL;
        private TrainingBL _trainingBL;

        private List<AppUserModel> _appUsers;
        private List<DepartmentModel> _departments;
        private List<EmployeeModel> _employees;
        private List<TrainingModel> _trainings;
        private List<EnrollmentModel> _enrollments;

        [SetUp]
        public void Setup()
        {
            _appUsers = Data.AppUsers;
            _departments = Data.Departments;
            _employees = Data.Employees;
            _enrollments = Data.Enrollments;
            _trainings = Data.Trainings;

            _stubAppUser = new Mock<IAppUserDAL>();
            _stubDepartment = new Mock<IDepartmentDAL>();
            _stubEmployee = new Mock<IEmployeeDAL>();
            _stubEnrollment = new Mock<IEnrollmentDAL>();
            _stubPrerequisite = new Mock<IPreRequisiteDAL>();
            _stubTraining = new Mock<ITrainingDAL>();


            _stubAppUser
                .Setup(a => a.GetHashedPasswordAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync((LoginViewModel model) => _appUsers.Where(a => a.Email.Equals(model.Email)).Select(a => a.Password).FirstOrDefault());

            _stubAppUser
                .Setup(a => a.RegisterUserAsync(It.IsAny<RegisterViewModel>()))
                .ReturnsAsync((RegisterViewModel model) =>
                {
                    _employees.Add(new EmployeeModel
                    {
                        EmployeeId = _employees.Count + 1,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        NIC = model.NIC,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        Department = _departments.FirstOrDefault(d => d.DepartmentId.Equals(model.DepartmentId)),
                    });
                    _appUsers.Add(new AppUserModel 
                    { 
                        AppUserId = _appUsers.Count + 1,
                        Email = model.Email,
                        Password = model.Password,
                        EmployeeId = _employees.Last().EmployeeId,
                    });
                    return true;
                });

            _stubDepartment
                .Setup(d => d.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => _departments.First(d => d.DepartmentId.Equals(id)));

            _stubEmployee
                .Setup(e => e.GetAllEmployeesAsync())
                .ReturnsAsync(_employees);

            _stubTraining
                .Setup(t => t.AutoCloseAsync())
                .ReturnsAsync(() =>
                {
                    return true;
                });
            _stubTraining
                .Setup(t => t.AddAsync(It.IsAny<TrainingViewModel>()))
                .ReturnsAsync((TrainingViewModel model) =>
                {
                    _trainings.Add(new TrainingModel
                    {
                        TrainingId = _trainings.Count + 1,
                        Title = model.Title,
                        Description = model.Description,
                        Deadline = model.Deadline,
                        Capacity = model.Capacity,
                        PriorityDepartment = model.PriorityDepartment,
                    });
                    return true;
                });
            _stubTraining
                .Setup(t => t.UpdateAsync(It.IsAny<TrainingModel>()))
                .ReturnsAsync((TrainingModel model) =>
                {
                    var trainingToUpdate = _trainings.FirstOrDefault(t => t.TrainingId.Equals(model.TrainingId));

                    if (trainingToUpdate != null)
                    {
                        trainingToUpdate.Title = model.Title;
                        trainingToUpdate.Description = model.Description;
                        trainingToUpdate.Deadline = model.Deadline;
                        trainingToUpdate.Capacity = model.Capacity;
                        trainingToUpdate.PriorityDepartment = model.PriorityDepartment;
                        trainingToUpdate.IsClosed = model.IsClosed;

                        return true;
                    }

                    return false;
                });
            _stubTraining
                .Setup(t => t.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var numOfTrainingDeleted = _trainings.RemoveAll(t => t.TrainingId.Equals(id));
                    return numOfTrainingDeleted == 1;
                });

            _appUserBL = new AppUserBL(_stubAppUser.Object, _stubEmployee.Object);
            _trainingBL = new TrainingBL(_stubTraining.Object, _stubDepartment.Object, _stubEnrollment.Object, _stubPrerequisite.Object);

        }

        [Test]
        [TestCase("john.doe@example.com", "4321", ExpectedResult = true)]
        [TestCase("john.doe@example.com", "0000", ExpectedResult = false)]
        [TestCase("notuser@example.com", "4321", ExpectedResult = false)]
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
        [TestCase("ved", "rowjee", 1, "john.doe@example.com", "1234", "R080901299930", "59749958", Role.Employee, ExpectedResult = "DuplicatedEmail")]
        [TestCase("ved", "rowjee", 1, "ved.rowjee@ceridian.com", "1234", "U1234567890123", "59749958", Role.Employee, ExpectedResult = "DuplicatedNIC")]
        [TestCase("john", "doe", 1, "john.doe@example.com", "1234", "U1234567890123", "59749958", Role.Employee, ExpectedResult = "DuplicatedEmail,DuplicatedNIC")]

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

        [Test]
        public async Task Test_AddTraining()
        {
            // Arrange
            var model = new TrainingViewModel()
            {
                Title = "Test",
                Description = "Test",
                Deadline = DateTime.Now.AddDays(4),
                Capacity = 5,
                PriorityDepartment = new DepartmentModel() { DepartmentId = 1, Title = "P&T" },
                PreRequisites = new List<string>(){ "HSC" }
            };
            // Act
            var result = await _trainingBL.AddTrainingAsync(model);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Test_UpdateTraining()
        {
            // Arrange
            int trainingIdToUpdate = 1;
            var model = _trainings.First(t => t.TrainingId.Equals(trainingIdToUpdate));
            var viewModel = new TrainingViewModel()
            {
                TrainingId = model.TrainingId,
                Title = model.Title,
                Description = model.Description,
                Deadline = model.Deadline,
                Capacity = model.Capacity,
                PriorityDepartment = model.PriorityDepartment,
                IsClosed = false,
            };
            var updatedTitle = "Updated Title";
            var updatedDescription = "Updated Description";

            viewModel.Title = updatedTitle;
            viewModel.Description = updatedDescription;

            // Act
            var result = await _trainingBL.UpdateTrainingAsync(viewModel);

            //Assert
            var updatedTraining = _trainings.FirstOrDefault(t => t.TrainingId.Equals(trainingIdToUpdate));

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result);

                Assert.NotNull(updatedTraining);
                Assert.AreEqual(updatedTraining.Title, updatedTitle);
                Assert.AreEqual(updatedTraining.Description, updatedDescription);
            });
            
        }

        [Test]
        public async Task Test_DeleteTraining()
        {
            // Arrange
            int trainingIdToDelete = 2;
            var model = _trainings.First(t => t.TrainingId.Equals(trainingIdToDelete));
            // Act
            var result = await _trainingBL.DeleteTrainingAsync(trainingIdToDelete);
            // Assert
            var deletedTraining = _trainings.FirstOrDefault(t => t.TrainingId.Equals(trainingIdToDelete));

            Assert.Multiple(() =>
            {
                Assert.IsTrue(result.IsSuccess);
                Assert.IsNull(deletedTraining);
            });
        }

        [Test]
        public async Task Test_AutomaticProcessing()
        {
            // Arrange

            // Act
            var result = await _trainingBL.AutoCloseTrainingAsync();

            //Assert
            Assert.IsTrue(result);
        }
    }
}