using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CryptoHelper;
using SkillsLabProject.BL.RepositoryBL;
using SkillsLabProject.DAL.Models;
using SkillsLabProject.Models.ViewModels;

namespace SkillsLabProject.BLL
{
    public interface IAppUserBL
    {
        bool AuthenticateUser(LoginViewModel model);
        string RegisterUser(RegisterViewModel model);
    }
    public class AppUserBL : IAppUserBL
    {
        private readonly IRepositoryBL<AppUser> _appUserRepositoryBL;
        private readonly IRepositoryBL<Employee> _employeeRepositoryBL;
        public AppUserBL(IRepositoryBL<AppUser> appUserDAL, IRepositoryBL<Employee> employeeDAL)
        {
            _appUserRepositoryBL = appUserDAL;
            _employeeRepositoryBL = employeeDAL;
        }

        public bool AuthenticateUser(LoginViewModel model)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("Email",model.Email)
            };
            var resultAppUser = _appUserRepositoryBL.GetAll();
            var appUserModel = resultAppUser.GetModelList().FirstOrDefault(x => x.Email.Equals(model.Email));
            var hashedPassword = appUserModel.Password;
            return hashedPassword != null && VerifyPassword(hashedPassword, model.Password);
        }
        public string RegisterUser(RegisterViewModel model)
        {
            try
            {
                if (!IsEmailAlreadyRegistered(model.Email))
                {
                    model.Password = HashPassword(model.Password);

                    var employeeModel = new Employee
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        NIC = model.NIC,
                        DepartmentId = model.DepartmentId,
                        RoleId = model.RoleId
                    };
                    var resultEmployee = _employeeRepositoryBL.Add(employeeModel); // not getting employeeId

                    var appUserModel = new AppUser
                    {
                        Email = model.Email,
                        Password = model.Password,
                        EmployeeId = resultEmployee.GetModel().EmployeeId,
                    };
                    var resultAppUser = _appUserRepositoryBL.Add(appUserModel);
                }
                return "DuplicatedEmail";
            }
            catch (Exception error)
            {
                return error.Message;
                throw;
            }
        }
        private bool IsEmailAlreadyRegistered(string email)
        {
            var appUserModels = _appUserRepositoryBL.GetAll().GetModelList();
            return appUserModels.Any(appUserModel => appUserModel.Email == email);
        }
        private string HashPassword(string password)
        {
            return Crypto.HashPassword(password);
        }
        private bool VerifyPassword(string hash, string password)
        {
            return Crypto.VerifyHashedPassword(hash, password);
        }
    }
}
