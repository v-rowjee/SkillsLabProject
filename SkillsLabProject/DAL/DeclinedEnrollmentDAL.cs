using SkillsLabProject.Models;
using SkillsLabProject.DAL.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL
{
    public interface IDeclinedEnrollmentDAL : IDAL<DeclinedEnrollmentModel>
    {

    }
    public class DeclinedEnrollmentDAL : IDeclinedEnrollmentDAL
    {
        private const string AddDeclinedEnrollmentQuery = @"
            INSERT [dbo].[DeclinedEnrollment] (EnrollmentId, Reason) VALUES (@EnrollmentId, @Reason);
        ";
        private const string GetAllDeclinedEnrollmentsQuery = @"
            SELECT DeclinedEnrollmentId, EnrollmentId, Reason
            FROM [dbo].[DeclinedEnrollment]
        ";
        private const string GetDeclinedEnrollmentQuery = @"
            SELECT DeclinedEnrollmentId, EnrollmentId, Reason
            FROM [dbo].[DeclinedEnrollment]
            WHERE [DeclinedEnrollmentId] = @DeclinedEnrollmentId
        ";
        private const string UpdateDeclinedEnrollmentQuery = @"
            UPDATE [dbo].[DeclinedEnrollment]
            SET EnrollmentId=@EnrollmentId, Reason=@Reason
            WHERE DeclinedEnrollmentId=@DeclinedEnrollmentId;
        ";
        private const string DeleteDeclinedEnrollmentQuery = @"
            DELETE FROM [dbo].[DeclinedEnrollment] WHERE DeclinedEnrollmentId=@DeclinedEnrollmentId
        ";

        public bool Add(DeclinedEnrollmentModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EnrollmentId", model.EnrollmentId));
            parameters.Add(new SqlParameter("@Reason", model.Reason));


            var DeclinedEnrollmentInserted = DBCommand.InsertUpdateData(AddDeclinedEnrollmentQuery, parameters);

            return DeclinedEnrollmentInserted;
        }

        public bool Delete(int declinedEnrollmentId)
        {
            var parameter = new SqlParameter("@DeclinedEnrollmentId", declinedEnrollmentId);
            return DBCommand.DeleteData(DeleteDeclinedEnrollmentQuery, parameter);
        }

        public IEnumerable<DeclinedEnrollmentModel> GetAll()
        {
            var DeclinedEnrollments = new List<DeclinedEnrollmentModel>();
            DeclinedEnrollmentModel DeclinedEnrollment;

            var dt = DBCommand.GetData(GetAllDeclinedEnrollmentsQuery);
            foreach (DataRow row in dt.Rows)
            {
                DeclinedEnrollment = new DeclinedEnrollmentModel();
                DeclinedEnrollment.DeclinedEnrollmentId = int.Parse(row["DeclinedEnrollmentId"].ToString());
                DeclinedEnrollment.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                DeclinedEnrollment.Reason = row["Reason"].ToString();

                DeclinedEnrollments.Add(DeclinedEnrollment);
            }
            return DeclinedEnrollments;
        }

        public DeclinedEnrollmentModel GetById(int declinedEnrollmentId)
        {
            var DeclinedEnrollment = new DeclinedEnrollmentModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DeclinedEnrollmentId", declinedEnrollmentId));

            var dt = DBCommand.GetDataWithCondition(GetDeclinedEnrollmentQuery, parameters);
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
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@DeclinedEnrollmentId", model.DeclinedEnrollmentId));
            parameters.Add(new SqlParameter("@EnrollmentId", model.EnrollmentId));
            parameters.Add(new SqlParameter("@Reason", model.Reason));

            var DeclinedEnrollmentUpdated = DBCommand.InsertUpdateData(UpdateDeclinedEnrollmentQuery, parameters);

            return DeclinedEnrollmentUpdated;
        }
    }
}
