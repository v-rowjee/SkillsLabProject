using SkillsLabProject.BLL;
using SkillsLabProject.DAL.Common;
using SkillsLabProject.Enums;
using SkillsLabProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL
{
    public interface IEnrollmentDAL : IDAL<EnrollmentModel>
    {

    }
    public class EnrollmentDAL : IEnrollmentDAL
    {
        private const string GetEnrollmentQuery = @"
            SELECT EnrollmentId, EmployeeId, EnrollmentId, StatusId
            FROM [dbo].[Enrollment]
            WHERE [EnrollmentId] = @EnrollmentId
        ";
        private const string GetAllEnrollmentsQuery = @"
            SELECT EnrollmentId, EmployeeId, EnrollmentId, StatusId
            FROM [dbo].[Enrollment]
        ";
        private const string AddEnrollmentQuery = @"
            INSERT [dbo].[Enrollment] (EmployeeId, EnrollmentId, StatusId) VALUES (@EmployeeId, @EnrollmentId, @StatusId);
        ";
        private const string UpdateEnrollmentQuery = @"
            UPDATE [dbo].[Enrollment]
            SET EmployeeId=@EmployeeId, EnrollmentId=@EnrollmentId, StatusId=@StatusId
            WHERE EnrollmentId=@EnrollmentId;
        ";
        private const string DeleteEnrollmentQuery = @"
            DELETE FROM [dbo].[DeclinedEnrollment] WHERE EnrollmentId=@EnrollmentId;
            DELETE FROM [dbo].[Proof] WHERE EnrollmentId=@EnrollmentId;
            DELETE FROM [dbo].[Enrollment] WHERE EnrollmentId=@EnrollmentId
        ";
        public IEnumerable<EnrollmentModel> GetAll()
        {
            var Enrollments = new List<EnrollmentModel>();
            EnrollmentModel Enrollment;

            var dt = DBCommand.GetData(GetAllEnrollmentsQuery);
            foreach (DataRow row in dt.Rows)
            {
                Enrollment = new EnrollmentModel();
                Enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Enrollment.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                Enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Enrollment.Status =  (StatusEnum) int.Parse(row["StatusId"].ToString());

                Enrollments.Add(Enrollment);
            }
            return Enrollments;
        }

        public EnrollmentModel GetById(int EnrollmentId)
        {
            var Enrollment = new EnrollmentModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EnrollmentId", EnrollmentId));

            var dt = DBCommand.GetDataWithCondition(GetEnrollmentQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                Enrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Enrollment.EmployeeId = int.Parse(row["EmployeeId"].ToString());
                Enrollment.TrainingId = int.Parse(row["TrainingId"].ToString());
                Enrollment.Status = (StatusEnum) int.Parse(row["PriorityDepartmentId"].ToString());
            }
            return Enrollment;
        }

        public bool Add(EnrollmentModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Title", model.EmployeeId));
            parameters.Add(new SqlParameter("@Deadline", model.TrainingId));
            parameters.Add(new SqlParameter("@Capacity", model.Status));

            var EnrollmentInserted = DBCommand.InsertUpdateData(AddEnrollmentQuery, parameters);

            return EnrollmentInserted;
        }

        public bool Update(EnrollmentModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EnrollmentId", model.EnrollmentId));
            parameters.Add(new SqlParameter("@Title", model.EmployeeId));
            parameters.Add(new SqlParameter("@Deadline", model.TrainingId));
            parameters.Add(new SqlParameter("@Capacity", model.Status));

            var EnrollmentUpdated = DBCommand.InsertUpdateData(UpdateEnrollmentQuery, parameters);

            return EnrollmentUpdated;
        }

        public bool Delete(int EnrollmentId)
        {
            var parameter = new SqlParameter("@EnrollmentId", EnrollmentId);
            return DBCommand.DeleteData(DeleteEnrollmentQuery, parameter);
        }

    }
}
