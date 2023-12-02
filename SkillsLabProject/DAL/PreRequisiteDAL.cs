using SkillsLabProject.DAL.Common;
using SkillsLabProject.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace SkillsLabProject.DAL
{
    public interface IPreRequisiteDAL : IDAL<PreRequisiteModel>
    {
    }
    public class PreRequisiteDAL : IPreRequisiteDAL
    {
        public bool Add(PreRequisiteModel model)
        {
            const string AddPreRequisiteQuery = @"
                INSERT [dbo].[PreRequisite] (Detail) VALUES (@Detail);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Detail", model.Detail)
            };
            return DBCommand.InsertUpdateData(AddPreRequisiteQuery, parameters);
        }
        public bool Delete(int PreRequisiteId)
        {
            const string DeletePreRequisiteQuery = @"
                DELETE FROM [dbo].[TrainingPreRequisite] WHERE PreRequisiteId=@PreRequisiteId;
                DELETE FROM [dbo].[PreRequisite] WHERE PreRequisiteId=@PreRequisiteId
            ";
            var parameter = new SqlParameter("@PreRequisiteId", PreRequisiteId);
            return DBCommand.DeleteData(DeletePreRequisiteQuery, parameter);
        }
        public IEnumerable<PreRequisiteModel> GetAll()
        {
            const string GetAllPreRequisitesQuery = @"
                SELECT PreRequisiteId, Detail
                FROM [dbo].[PreRequisite]
            ";
            var dt = DBCommand.GetData(GetAllPreRequisitesQuery);
            var PreRequisites = new List<PreRequisiteModel>();
            PreRequisiteModel PreRequisite;
            foreach (DataRow row in dt.Rows)
            {
                PreRequisite = new PreRequisiteModel();
                PreRequisite.PreRequisiteId = int.Parse(row["PreRequisiteId"].ToString());
                PreRequisite.Detail = row["Title"].ToString();

                PreRequisites.Add(PreRequisite);
            }
            return PreRequisites;
        }
        public PreRequisiteModel GetById(int PreRequisiteId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@PreRequisiteId", PreRequisiteId)
            };
            const string GetPreRequisiteQuery = @"
            SELECT PreRequisiteId, Detail
            FROM [dbo].[PreRequisite]
            WHERE [PreRequisiteId] = @PreRequisiteId
        ";
            var dt = DBCommand.GetDataWithCondition(GetPreRequisiteQuery, parameters);
            var PreRequisite = new PreRequisiteModel();
            foreach (DataRow row in dt.Rows)
            {
                PreRequisite.PreRequisiteId = int.Parse(row["PreRequisiteId"].ToString());
                PreRequisite.Detail = row["Title"].ToString();
            }
            return PreRequisite;
        }
        public bool Update(PreRequisiteModel model)
        {
            const string UpdatePreRequisiteQuery = @"
                UPDATE [dbo].[PreRequisite]
                SET Detail=@Detail
                WHERE PreRequisiteId=@PreRequisiteId;
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@PreRequisiteId", model.PreRequisiteId),
                new SqlParameter("@Detail", model.Detail)
            };
            return DBCommand.InsertUpdateData(UpdatePreRequisiteQuery, parameters);
        }
    }
}