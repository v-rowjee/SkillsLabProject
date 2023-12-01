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
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", model.Email));
            parameters.Add(new SqlParameter("@Password", model.Password));
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
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", model.Email));
            parameters.Add(new SqlParameter("@Password", model.Password));
            parameters.Add(new SqlParameter("@FirstName", model.FirstName));
            parameters.Add(new SqlParameter("@LastName", model.LastName));
            parameters.Add(new SqlParameter("@NIC", model.NIC));
            parameters.Add(new SqlParameter("@PhoneNumber", model.PhoneNumber));
            parameters.Add(new SqlParameter("@DepartmentId", model.DepartmentId));
            parameters.Add(new SqlParameter("@RoleId", (int) model.Role));
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
            string hashedPassword = "";
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Email", model.Email));
            const string GET_HASHED_PASSWORD_QUERY = @"
                SELECT Password 
                FROM [dbo].[AppUser] a 
                INNER JOIN Employee e ON a.EmployeeId=e.EmployeeId
                WHERE a.Email=@Email
            ";
            var dt = DBCommand.GetDataWithCondition(GET_HASHED_PASSWORD_QUERY, parameters);
            foreach (DataRow row in dt.Rows)
            {
                hashedPassword = row["Password"].ToString();
            }
            return hashedPassword;
        }
        public IEnumerable<string> GetAllEmails()
        {
            var emails = new List<string>();
            const string GET_ALL_EMAIL_SQUERY = @"SELECT Email FROM [dbo].[AppUser]";
            var dt = DBCommand.GetData(GET_ALL_EMAIL_SQUERY);
            foreach (DataRow row in dt.Rows)
            {
                emails.Add(row["Email"].ToString());
            }
            return emails;
        }
    }
}