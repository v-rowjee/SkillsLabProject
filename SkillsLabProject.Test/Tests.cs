using Moq;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.Test
{
    public class Tests
    {
        private Mock<IAppUserDAL> _stub;

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
            _stub = new Mock<IAppUserDAL>();
            _stub.Setup(iappuserdal => iappuserdal.AuthenticateUserAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(true);
        }

        [Test]
        public void Login()
        {
            
            Assert.Pass();
        }
    }
}