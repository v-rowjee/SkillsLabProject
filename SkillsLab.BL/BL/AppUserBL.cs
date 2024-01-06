using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoHelper;
using SkillsLabProject.Common.Custom;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.BL.BL
{
    public interface IAppUserBL
    {
        Task<Result> AuthenticateUserAsync(LoginViewModel model);
        Task<List<string>> RegisterUserAsync(RegisterViewModel model);
    }

    public class AppUserBL : IAppUserBL
    {
        private readonly IAppUserDAL _appUserDAL;
        private readonly IEmployeeDAL _employeeDAL;

        public AppUserBL(IAppUserDAL appUserDAL, IEmployeeDAL employeeDAL)
        {
            _appUserDAL = appUserDAL;
            _employeeDAL = employeeDAL;
        }

        public async Task<Result> AuthenticateUserAsync(LoginViewModel model)
        {
            var hashedPassword = await _appUserDAL.GetHashedPasswordAsync(model);
            var isAuthenticated = hashedPassword != null && VerifyPassword(hashedPassword, model.Password);

            return new Result()
            {
                IsSuccess = isAuthenticated,
                Message = isAuthenticated ? "Authentication successful!" : "Unable to authenticate."
            };
        }

        public async Task<List<string>> RegisterUserAsync(RegisterViewModel model)
        {
            var validations = await ValidateUserAsync(model);

            if (validations.All(validation => validation == "Success"))
            {
                model.Password = HashPassword(model.Password);
                return await _appUserDAL.RegisterUserAsync(model) ? new List<string> { "Success" } : new List<string> { "Error" };
            }

            return validations;
        }

        private async Task<List<string>> ValidateUserAsync(RegisterViewModel model)
        {
            var employees = (await _employeeDAL.GetAllEmployeesAsync()).ToList();

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
