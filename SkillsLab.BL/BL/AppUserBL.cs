using System;
using System.Collections.Generic;
using System.Linq;
using CryptoHelper;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.BL.BL
{
    public interface IAppUserBL
    {
        bool AuthenticateUser(LoginViewModel model);
        List<string> RegisterUser(RegisterViewModel model);
    }
    public class AppUserBL : IAppUserBL
    {
        private readonly IAppUserDAL _appUserDAL;
        private readonly IEmployeeDAL _employeeDAL;
        public AppUserBL(IAppUserDAL appUserBL, IEmployeeDAL employeeDAL)
        {
            _appUserDAL = appUserBL;
            _employeeDAL = employeeDAL;
        }
        public bool AuthenticateUser(LoginViewModel model)
        {
            var hashedPassword = _appUserDAL.GetHashedPassword(model);
            return hashedPassword != null && VerifyPassword(hashedPassword, model.Password);
        }
        public List<string> RegisterUser(RegisterViewModel model)
        {
            var validations = ValidateUser(model);

            if (validations.All(validation => validation == "Success"))
            {
                model.Password = HashPassword(model.Password);
                return _appUserDAL.RegisterUser(model) ? new List<string> { "Success" } : new List<string> { "Error" };
            }
            return validations;
        }

        private List<string> ValidateUser(RegisterViewModel model)
        {
            var employees = _employeeDAL.GetAllEmployees().ToList();

            var validationErrors = new List<string>();

            if (employees.Any(e => e.Email == model.Email.Trim()))
            {
                validationErrors.Add("DuplicatedEmail");
            }

            if (employees.Any(e => e.NIC == model.NIC.Trim()))
            {
                validationErrors.Add("DuplicatedNIC");
            }

            if (employees.Any(e => e.PhoneNumber == model.PhoneNumber.Trim()))
            {
                validationErrors.Add("DuplicatedPhoneNumber");
            }

            return validationErrors.Any() ? validationErrors : new List<string> { "Success" };
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
