using SkillsLabProject.DAL.Common;
using SkillsLabProject.Models.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace SkillsLabProject.DAL
{
    public interface IAppUserDAL
    {
        bool AuthenticateUser(LoginViewModel login);
        bool RegisterUser(RegisterViewModel registration);
        IEnumerable<string> GetAllEmails();
        string GetHashedPassword(LoginViewModel login);
    }
    public class AppUserDAL : IAppUserDAL
    {
        public bool AuthenticateUser(LoginViewModel login)
        {
            const string AuthenticateUserQuery = @"
                SELECT e.EmployeeId
                FROM [dbo].[Employee] e 
                INNER JOIN [dbo].[AppUser] a ON e.[EmployeeId]=a.[EmployeeId] 
                WHERE a.[Email] = @Email AND a.[Password] = @Password 
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", login.Email),
                new SqlParameter("@Password", login.Password)
            };
            var dt = DBCommand.GetDataWithCondition(AuthenticateUserQuery, parameters);
            return dt.Rows.Count > 0;
        }

        public bool RegisterUser(RegisterViewModel registration)
        {
            const string RegisterUserQuery = @"
                INSERT [dbo].[Employee] ([FirstName] ,[LastName] ,[NIC] ,[PhoneNumber], [DepartmentId], [RoleId])
                VALUES (@FirstName ,@LastName, @NIC, @PhoneNumber, @DepartmentId, @RoleId);

                INSERT [dbo].[AppUser] (Email, Password, EmployeeId)
                VALUES (@Email, @Password, @@IDENTITY)
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
            return DBCommand.InsertUpdateData(RegisterUserQuery, parameters);
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
        public string GetHashedPassword(LoginViewModel login)
        {
            const string GetHashedPasswordQuery = @"
                SELECT Password 
                FROM [dbo].[AppUser] a 
                INNER JOIN Employee e ON a.EmployeeId=e.EmployeeId
                WHERE a.Email=@Email
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", login.Email)
            };
            var dt = DBCommand.GetDataWithCondition(GetHashedPasswordQuery, parameters);
            string hashedPassword = "";
            foreach (DataRow row in dt.Rows)
            {
                hashedPassword = row["Password"].ToString();
            }
            return hashedPassword;
        }
    }
}