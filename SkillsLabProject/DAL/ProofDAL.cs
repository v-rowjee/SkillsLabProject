using SkillsLabProject.Models;
using SkillsLabProject.DAL.Common;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
namespace SkillsLabProject.DAL
{
    public interface IProofDAL : IDAL<ProofModel>
    {
    }
    public class ProofDAL : IProofDAL
    {
        public bool Add(ProofModel model)
        {
            const string AddProofQuery = @"
                INSERT [dbo].[Proof] (EnrollmentId, Attachment) VALUES (@EnrollmentId, @Attachment);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EnrollmentId", model.EnrollmentId),
                new SqlParameter("@Attachment", model.Attachment)
            };
            return DBCommand.InsertUpdateData(AddProofQuery, parameters);
        }
        public bool Delete(int ProofId)
        {
            const string DeleteProofQuery = @"
                DELETE FROM [dbo].[Proof] WHERE ProofId=@ProofId
            ";
            var parameter = new SqlParameter("@ProofId", ProofId);
            return DBCommand.DeleteData(DeleteProofQuery, parameter);
        }
        public IEnumerable<ProofModel> GetAll()
        {
            const string GetAllProofsQuery = @"
                SELECT ProofId, EnrollmentId, Attachment
                FROM [dbo].[Proof]
            ";
            var dt = DBCommand.GetData(GetAllProofsQuery);
            var Proofs = new List<ProofModel>();
            ProofModel Proof;
            foreach (DataRow row in dt.Rows)
            {
                Proof = new ProofModel();
                Proof.ProofId = int.Parse(row["ProofId"].ToString());
                Proof.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Proof.Attachment = row["Attachment"].ToString();

                Proofs.Add(Proof);
            }
            return Proofs;
        }
        public ProofModel GetById(int ProofId)
        {
            const string GetProofQuery = @"
                SELECT ProofId, EnrollmentId, Attachment
                FROM [dbo].[Proof]
                WHERE [ProofId] = @ProofId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ProofId", ProofId)
            };
            var dt = DBCommand.GetDataWithCondition(GetProofQuery, parameters);
            var Proof = new ProofModel();
            foreach (DataRow row in dt.Rows)
            {
                Proof.ProofId = int.Parse(row["ProofId"].ToString());
                Proof.EnrollmentId = int.Parse(row["EnrollmentId"].ToString());
                Proof.Attachment = row["Attachment"].ToString();
            }
            return Proof;
        }
        public bool Update(ProofModel model)
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
            return DBCommand.InsertUpdateData(UpdateProofQuery, parameters);
        }
    }
}