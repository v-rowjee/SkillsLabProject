using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoHelper;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.BL.BL
{
    public interface IAppUserBL
    {
        bool AuthenticateUser(LoginViewModel model);
        string RegisterUser(RegisterViewModel model);
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
        public string RegisterUser(RegisterViewModel model)
        {
            try
            {
                if (!IsEmailAlreadyRegistered(model.Email))
                {
                    model.Password = HashPassword(model.Password);
                    return _appUserDAL.RegisterUser(model) ? "Success" : "Error";
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
            return _appUserDAL.GetAllEmails().Contains(email.Trim());
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
