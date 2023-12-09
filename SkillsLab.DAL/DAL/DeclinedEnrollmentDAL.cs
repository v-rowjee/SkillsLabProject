using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.DAL;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SkillsLabProject.Common.Models;

namespace SkillsLabProject.DAL.DAL
{
    public interface IDeclinedEnrollmentDAL : IDAL<DeclinedEnrollmentModel>
    {
    }
    public class DeclinedEnrollmentDAL : IDeclinedEnrollmentDAL
    {
        public bool Add(DeclinedEnrollmentModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EnrollmentId", model.EnrollmentId));
            parameters.Add(new SqlParameter("@Reason", model.Reason));
            const string AddDeclinedEnrollmentQuery = @"
                INSERT [dbo].[DeclinedEnrollment] (EnrollmentId, Reason) VALUES (@EnrollmentId, @Reason);
            ";
            return DBCommand.InsertUpdateData(AddDeclinedEnrollmentQuery, parameters);
        }
        public bool Delete(int declinedEnrollmentId)
        {
            const string DeleteDeclinedEnrollmentQuery = @"
                DELETE FROM [dbo].[DeclinedEnrollment] 
                WHERE DeclinedEnrollmentId=@DeclinedEnrollmentId
            ";
            var parameter = new SqlParameter("@DeclinedEnrollmentId", declinedEnrollmentId);
            return DBCommand.DeleteData(DeleteDeclinedEnrollmentQuery, parameter);
        }
        public IEnumerable<DeclinedEnrollmentModel> GetAll()
        {
            const string GetAllDeclinedEnrollmentsQuery = @"
                SELECT DeclinedEnrollmentId, EnrollmentId, Reason
                FROM [dbo].[DeclinedEnrollment]
            ";
            var dt = DBCommand.GetData(GetAllDeclinedEnrollmentsQuery);
            var DeclinedEnrollments = new List<DeclinedEnrollmentModel>();
            DeclinedEnrollmentModel DeclinedEnrollment;
            foreach (DataRow row in dt.Rows)
            {
                DeclinedEnrollment = new DeclinedEnrollmentModel
                {
                    DeclinedEnrollmentId = int.Parse(row["DeclinedEnrollmentId"].ToString()),
                    EnrollmentId = int.Parse(row["EnrollmentId"].ToString()),
                    Reason = row["Reason"].ToString()
                };
                DeclinedEnrollments.Add(DeclinedEnrollment);
            }
            return DeclinedEnrollments;
        }
        public DeclinedEnrollmentModel GetById(int declinedEnrollmentId)
        {
            const string GetDeclinedEnrollmentQuery = @"
                SELECT DeclinedEnrollmentId, EnrollmentId, Reason
                FROM [dbo].[DeclinedEnrollment]
                WHERE [DeclinedEnrollmentId] = @DeclinedEnrollmentId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DeclinedEnrollmentId", declinedEnrollmentId)
            };
            var dt = DBCommand.GetDataWithCondition(GetDeclinedEnrollmentQuery, parameters);
            var DeclinedEnrollment = new DeclinedEnrollmentModel();
            foreach (DataRow row in dt.Rows)
            {
                DeclinedEnrollment.DeclinedEnrollmentId = int.Parse(row["DeclinedEnrollmentId"].ToString());
                DeclinedEnrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                DeclinedEnrollment.Reason = row["Reason"].ToString();
            }
            return DeclinedEnrollment;
        }
        public bool Update(DeclinedEnrollmentModel model)
        {
            const string UpdateDeclinedEnrollmentQuery = @"
                UPDATE [dbo].[DeclinedEnrollment]
                SET EnrollmentId=@EnrollmentId, Reason=@Reason
                WHERE DeclinedEnrollmentId=@DeclinedEnrollmentId;
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@DeclinedEnrollmentId", model.DeclinedEnrollmentId),
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@Reason", model.Reason)
            };
            return DBCommand.InsertUpdateData(UpdateDeclinedEnrollmentQuery, parameters);
        }
    }
}
