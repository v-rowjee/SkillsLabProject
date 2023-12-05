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
        private readonly IRepositoryBL<AppUser> _appUserDAL;
        private readonly IRepositoryBL<Employee> _employeeDAL;
        public AppUserBL(IRepositoryBL<AppUser> appUserDAL, IRepositoryBL<Employee> employeeDAL)
        {
            _appUserDAL = appUserDAL;
            _employeeDAL = employeeDAL;
        }

        public bool AuthenticateUser(LoginViewModel model)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("Email",model.Email)
            };
            var resultAppUser = _appUserDAL.GetAll();
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
                        Role = model.Role
                    };
                    var resultEmployee = _employeeDAL.Add(employeeModel);

                    var appUserModel = new AppUser
                    {
                        Email = model.Email,
                        Password = model.Password,
                        EmployeeId = resultEmployee.GetModel().EmployeeId,
                    };
                    var resultAppUser = _appUserDAL.Add(appUserModel);
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
            var appUserModels = _appUserDAL.GetAll().GetModelList();
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
