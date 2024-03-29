﻿using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface IAppUserDAL
    {
        Task<bool> AuthenticateUserAsync(LoginViewModel login);
        Task<bool> RegisterUserAsync(RegisterViewModel registration);
        Task<IEnumerable<string>> GetAllEmailsAsync();
        Task<string> GetHashedPasswordAsync(LoginViewModel login);
    }
    public class AppUserDAL : IAppUserDAL
    {
        public async Task<bool> AuthenticateUserAsync(LoginViewModel login)
        {
            const string AuthenticateUserQuery = @"
                SELECT e.EmployeeId
                FROM [dbo].[Employee] e 
                INNER JOIN [dbo].[AppUser] a ON e.[EmployeeId] = a.[EmployeeId] 
                WHERE a.[Email] = @Email AND a.[Password] = @Password 
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", login.Email),
                new SqlParameter("@Password", login.Password)
            };

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(AuthenticateUserQuery, parameters).ConfigureAwait(false))
            {
                return await dataReader.ReadAsync().ConfigureAwait(false);
            }
        }


        public async Task<bool> RegisterUserAsync(RegisterViewModel registration)
        {
            const string RegisterUserQuery = @"
                BEGIN TRANSACTION
                    INSERT [dbo].[Employee] ([FirstName] ,[LastName] ,[NIC] ,[PhoneNumber], [DepartmentId])
                    VALUES (@FirstName ,@LastName, @NIC, @PhoneNumber, @DepartmentId);

                    INSERT [dbo].[AppUser] (Email, Password, EmployeeId)
                    VALUES (@Email, @Password, @@IDENTITY)

                    INSERT [dbo].[UserRole] (AppUserId, RoleId)
                    VALUES (@@IDENTITY, @RoleId)
                COMMIT
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", registration.Email),
                new SqlParameter("@Password", registration.Password),
                new SqlParameter("@FirstName", registration.FirstName),
                new SqlParameter("@LastName", registration.LastName),
                new SqlParameter("@NIC", registration.NIC),
                new SqlParameter("@PhoneNumber", registration.PhoneNumber),
                new SqlParameter("@DepartmentId", registration.DepartmentId),
                new SqlParameter("@RoleId", (int)registration.Role)
            };
            return await DBCommand.InsertDataAsync(RegisterUserQuery, parameters).ConfigureAwait(false);
        }
        public async Task<IEnumerable<string>> GetAllEmailsAsync()
        {
            const string GetAllEmailsQuery = @"SELECT Email FROM [dbo].[AppUser]";

            var emails = new List<string>();

            using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllEmailsQuery).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    string email = dataReader["Email"].ToString();
                    emails.Add(email);
                }
            }

            return emails;
        }
        public async Task<string> GetHashedPasswordAsync(LoginViewModel login)
        {
            const string GetHashedPasswordQuery = @"
                SELECT Password 
                FROM [dbo].[AppUser] a 
                INNER JOIN Employee e ON a.EmployeeId = e.EmployeeId
                WHERE a.Email = @Email
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", login.Email)
            };

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetHashedPasswordQuery, parameters).ConfigureAwait(false))
            {
                if (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    return dataReader["Password"].ToString();
                }
            }
            return null;
        }
    }
}