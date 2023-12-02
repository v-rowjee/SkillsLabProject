using SkillsLabProject.DAL.Common;
using SkillsLabProject.Enums;
using SkillsLabProject.Models;
using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SkillsLabProject.DAL
{
    public interface IAppUserDAL
    {
        bool AuthenticateUser(LoginViewModel model);
        bool RegisterUser(RegisterViewModel model);
        string GetHashedPassword(LoginViewModel model);
        IEnumerable<string> GetAllEmails();
    }
    public class AppUserDAL : IAppUserDAL
    {
        public bool AuthenticateUser(LoginViewModel model)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@Password", model.Password)
            };
            const string AUTHENTICATE_USER_QUERY = @"
                SELECT e.EmployeeId
                FROM [dbo].[Employee] e 
                INNER JOIN [dbo].[AppUser] a ON e.[EmployeeId]=a.[EmployeeId] 
                WHERE a.[Email] = @Email AND a.[Password] = @Password 
            ";
            var dt = DBCommand.GetDataWithCondition(AUTHENTICATE_USER_QUERY, parameters);
            return dt.Rows.Count > 0;
        }

        public bool RegisterUser(RegisterViewModel model)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@Password", model.Password),
                new SqlParameter("@FirstName", model.FirstName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@NIC", model.NIC),
                new SqlParameter("@PhoneNumber", model.PhoneNumber),
                new SqlParameter("@DepartmentId", model.DepartmentId),
                new SqlParameter("@RoleId", (int)model.Role)
            };
            const string REGISTER_USER_QUERY = @"
                INSERT [dbo].[Employee] ([FirstName] ,[LastName] ,[NIC] ,[PhoneNumber], [DepartmentId], [RoleId])
                VALUES (@FirstName ,@LastName, @NIC, @PhoneNumber, @DepartmentId, @RoleId);

                INSERT [dbo].[AppUser] (Email, Password, EmployeeId)
                VALUES (@Email, @Password, @@IDENTITY)
            ";
            return DBCommand.InsertUpdateData(REGISTER_USER_QUERY, parameters);
        }
        public string GetHashedPassword(LoginViewModel model)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", model.Email)
            };
            const string GET_HASHED_PASSWORD_QUERY = @"
                SELECT Password 
                FROM [dbo].[AppUser] a 
                INNER JOIN Employee e ON a.EmployeeId=e.EmployeeId
                WHERE a.Email=@Email
            ";
            var dt = DBCommand.GetDataWithCondition(GET_HASHED_PASSWORD_QUERY, parameters);
            string hashedPassword = "";
            foreach (DataRow row in dt.Rows)
            {
                hashedPassword = row["Password"].ToString();
            }
            return hashedPassword;
        }
        public IEnumerable<string> GetAllEmails()
        {
            const string GET_ALL_EMAILS_QUERY = @"SELECT Email FROM [dbo].[AppUser]";
            var dt = DBCommand.GetData(GET_ALL_EMAILS_QUERY);
            var emails = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                emails.Add(row["Email"].ToString());
            }
            return emails;
        }
    }
}