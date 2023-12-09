using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace SkillsLabProject.DAL.DAL
{
    public interface IEnrollmentDAL : IDAL<EnrollmentModel>
    {
    }
    public class EnrollmentDAL : IEnrollmentDAL
    {
        public IEnumerable<EnrollmentModel> GetAll()
        {
            const string GetAllEnrollmentsQuery = @"
                SELECT EnrollmentId, EmployeeId, EnrollmentId, StatusId
                FROM [dbo].[Enrollment]
            ";
            var dt = DBCommand.GetData(GetAllEnrollmentsQuery);
            var Enrollments = new List<EnrollmentModel>();
            EnrollmentModel Enrollment;
            foreach (DataRow row in dt.Rows)
            {
                Enrollment = new EnrollmentModel();
                Enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Enrollment.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                Enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Enrollment.Status =  (Status) int.Parse(row["StatusId"].ToString());

                Enrollments.Add(Enrollment);
            }
            return Enrollments;
        }
        public EnrollmentModel GetById(int EnrollmentId)
        {
            const string GetEnrollmentQuery = @"
                SELECT EnrollmentId, EmployeeId, EnrollmentId, StatusId
                FROM [dbo].[Enrollment]
                WHERE [EnrollmentId] = @EnrollmentId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", EnrollmentId)
            };
            var dt = DBCommand.GetDataWithCondition(GetEnrollmentQuery, parameters);
            var enrollment = new EnrollmentModel();
            foreach (DataRow row in dt.Rows)
            {
                enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                enrollment.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                enrollment.TrainingId = int.Parse(row["TrainingId"].ToString());
                enrollment.Status = (Status) int.Parse(row["PriorityDepartmentId"].ToString());
            }
            return enrollment;
        }
        public bool Add(EnrollmentModel model)
        {
            const string AddEnrollmentQuery = @"
                INSERT [dbo].[Enrollment] (EmployeeId, EnrollmentId, StatusId) VALUES (@EmployeeId, @EnrollmentId, @StatusId);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Title", model.EmployeeId),
                new SqlParameter("@Deadline", model.TrainingId),
                new SqlParameter("@Capacity", model.Status)
            };
            return DBCommand.InsertUpdateData(AddEnrollmentQuery, parameters);
        }
        public bool Update(EnrollmentModel model)
        {
            const string UpdateEnrollmentQuery = @"
                UPDATE [dbo].[Enrollment]
                SET EmployeeId=@EmployeeId, EnrollmentId=@EnrollmentId, StatusId=@StatusId
                WHERE EnrollmentId=@EnrollmentId;
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@Title", model.EmployeeId),
                new SqlParameter("@Deadline", model.TrainingId),
                new SqlParameter("@Capacity", model.Status)
            };
            return DBCommand.InsertUpdateData(UpdateEnrollmentQuery, parameters);
        }
        public bool Delete(int EnrollmentId)
        {
            const string DeleteEnrollmentQuery = @"
                BEGIN TRANSACTION
                    DELETE FROM [dbo].[DeclinedEnrollment] WHERE EnrollmentId=@EnrollmentId;
                    DELETE FROM [dbo].[Proof] WHERE EnrollmentId=@EnrollmentId;
                    DELETE FROM [dbo].[Enrollment] WHERE EnrollmentId=@EnrollmentId
                COMMIT
            ";
            var parameter = new SqlParameter("@EnrollmentId", EnrollmentId);
            return DBCommand.DeleteData(DeleteEnrollmentQuery, parameter);
        }
    }
}