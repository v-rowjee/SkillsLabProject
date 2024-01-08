using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface IDeclinedEnrollmentDAL : IDAL<DeclinedEnrollmentModel>
    {
    }
    public class DeclinedEnrollmentDAL : IDeclinedEnrollmentDAL
    {
        public async Task<bool> AddAsync(DeclinedEnrollmentModel model)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@Reason", model.Reason)
            };
            const string AddDeclinedEnrollmentQuery = @"
                INSERT [dbo].[DeclinedEnrollment] (EnrollmentId, Reason) VALUES (@EnrollmentId, @Reason);
            ";
            return await DBCommand.InsertDataAsync(AddDeclinedEnrollmentQuery, parameters).ConfigureAwait(false);
        }
        public async Task<bool> DeleteAsync(int declinedEnrollmentId)
        {
            const string DeleteDeclinedEnrollmentQuery = @"
                DELETE FROM [dbo].[DeclinedEnrollment] 
                WHERE DeclinedEnrollmentId=@DeclinedEnrollmentId
            ";
            var parameter = new SqlParameter("@DeclinedEnrollmentId", declinedEnrollmentId);
            return await DBCommand.DeleteDataAsync(DeleteDeclinedEnrollmentQuery, parameter).ConfigureAwait(false);
        }
        public async Task<IEnumerable<DeclinedEnrollmentModel>> GetAllAsync()
        {
            const string GetAllDeclinedEnrollmentsQuery = @"
                SELECT DeclinedEnrollmentId, EnrollmentId, Reason
                FROM [dbo].[DeclinedEnrollment]
            ";

            var DeclinedEnrollments = new List<DeclinedEnrollmentModel>();

            try
            {
                using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllDeclinedEnrollmentsQuery).ConfigureAwait(false))
                {
                    while (await dataReader.ReadAsync().ConfigureAwait(false))
                    {
                        DeclinedEnrollmentModel DeclinedEnrollment = new DeclinedEnrollmentModel
                        {
                            DeclinedEnrollmentId = dataReader.GetInt16(dataReader.GetOrdinal("DeclinedEnrollmentId")),
                            EnrollmentId = dataReader.GetInt16(dataReader.GetOrdinal("EnrollmentId")),
                            Reason = dataReader["Reason"].ToString()
                        };
                        DeclinedEnrollments.Add(DeclinedEnrollment);
                    }
                }
            }
            catch
            {
                throw;
            }

            return DeclinedEnrollments;
        }
        public async Task<DeclinedEnrollmentModel> GetByIdAsync(int declinedEnrollmentId)
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

            var DeclinedEnrollment = new DeclinedEnrollmentModel();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetDeclinedEnrollmentQuery, parameters).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    DeclinedEnrollment.DeclinedEnrollmentId = dataReader.GetInt16(dataReader.GetOrdinal("DeclinedEnrollmentId"));
                    DeclinedEnrollment.EnrollmentId = dataReader.GetInt16(dataReader.GetOrdinal("EnrollmentId"));
                    DeclinedEnrollment.Reason = dataReader["Reason"].ToString();
                }
            }


            return DeclinedEnrollment;
        }
        public async Task<bool> UpdateAsync(DeclinedEnrollmentModel model)
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
            return await DBCommand.UpdateDataAsync(UpdateDeclinedEnrollmentQuery, parameters).ConfigureAwait(false);
        }
    }
}
