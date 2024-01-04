using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface IProofDAL : IDAL<ProofModel>
    {
    }
    public class ProofDAL : IProofDAL
    {
        public async Task<bool> AddAsync(ProofModel model)
        {
            const string AddProofQuery = @"
                INSERT [dbo].[Proof] (EnrollmentId, Attachment) VALUES (@EnrollmentId, @Attachment);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@Attachment", model.Attachment)
            };

            return await DBCommand.InsertDataAsync(AddProofQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(int proofId)
        {
            const string DeleteProofQuery = @"
                DELETE FROM [dbo].[Proof] WHERE ProofId=@ProofId
            ";
            var parameter = new SqlParameter("@ProofId", proofId);

            return await DBCommand.DeleteDataAsync(DeleteProofQuery, parameter).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProofModel>> GetAllAsync()
        {
            const string GetAllProofsQuery = @"
                SELECT ProofId, EnrollmentId, Attachment
                FROM [dbo].[Proof]
            ";

            var proofs = new List<ProofModel>();

            using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllProofsQuery))
            {
                while (await dataReader.ReadAsync())
                {
                    var proof = new ProofModel
                    {
                        ProofId = dataReader.GetInt32(dataReader.GetOrdinal("ProofId")),
                        EnrollmentId = dataReader.GetInt32(dataReader.GetOrdinal("EnrollmentId")),
                        Attachment = dataReader.GetString(dataReader.GetOrdinal("Attachment"))
                    };

                    proofs.Add(proof);
                }
            }

            return proofs;
        }

        public async Task<ProofModel> GetByIdAsync(int proofId)
        {
            const string GetProofQuery = @"
                SELECT ProofId, EnrollmentId, Attachment
                FROM [dbo].[Proof]
                WHERE [ProofId] = @ProofId
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProofId", proofId)
            };

            var proof = new ProofModel();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetProofQuery, parameters))
            {
                while (await dataReader.ReadAsync())
                {
                    proof.ProofId = dataReader.GetInt32(dataReader.GetOrdinal("ProofId"));
                    proof.EnrollmentId = dataReader.GetInt32(dataReader.GetOrdinal("EnrollmentId"));
                    proof.Attachment = dataReader.GetString(dataReader.GetOrdinal("Attachment"));
                }
            }

            return proof;
        }

        public async Task<bool> UpdateAsync(ProofModel model)
        {
            const string UpdateProofQuery = @"
                UPDATE [dbo].[Proof]
                SET EnrollmentId=@EnrollmentId, Attachment=@Attachment
                WHERE ProofId=@ProofId;
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProofId", model.ProofId),
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@Attachment", model.Attachment)
            };

            return await DBCommand.UpdateDataAsync(UpdateProofQuery, parameters).ConfigureAwait(false);
        }

    }
}