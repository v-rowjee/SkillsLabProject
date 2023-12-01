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
    public interface IProofDAL : IDAL<ProofModel>
    {

    }
    public class ProofDAL : IProofDAL
    {
        private const string AddProofQuery = @"
            INSERT [dbo].[Proof] (EnrollmentId, Attachment) VALUES (@EnrollmentId, @Attachment);
        ";
        private const string GetAllProofsQuery = @"
            SELECT ProofId, EnrollmentId, Attachment
            FROM [dbo].[Proof]
        ";
        private const string GetProofQuery = @"
            SELECT ProofId, EnrollmentId, Attachment
            FROM [dbo].[Proof]
            WHERE [ProofId] = @ProofId
        ";
        private const string UpdateProofQuery = @"
            UPDATE [dbo].[Proof]
            SET EnrollmentId=@EnrollmentId, Attachment=@Attachment
            WHERE ProofId=@ProofId;
        ";
        private const string DeleteProofQuery = @"
            DELETE FROM [dbo].[Proof] WHERE ProofId=@ProofId
        ";

        public bool Add(ProofModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EnrollmentId", model.EnrollmentId));
            parameters.Add(new SqlParameter("@Attachment", model.Attachment));


            var ProofInserted = DBCommand.InsertUpdateData(AddProofQuery, parameters);

            return ProofInserted;
        }

        public bool Delete(int ProofId)
        {
            var parameter = new SqlParameter("@ProofId", ProofId);
            return DBCommand.DeleteData(DeleteProofQuery, parameter);
        }

        public IEnumerable<ProofModel> GetAll()
        {
            var Proofs = new List<ProofModel>();
            ProofModel Proof;

            var dt = DBCommand.GetData(GetAllProofsQuery);
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
            var Proof = new ProofModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ProofId", ProofId));

            var dt = DBCommand.GetDataWithCondition(GetProofQuery, parameters);
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
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ProofId", model.ProofId));
            parameters.Add(new SqlParameter("@EnrollmentId", model.EnrollmentId));
            parameters.Add(new SqlParameter("@Attachment", model.Attachment));

            var ProofUpdated = DBCommand.InsertUpdateData(UpdateProofQuery, parameters);

            return ProofUpdated;
        }
    }
}
