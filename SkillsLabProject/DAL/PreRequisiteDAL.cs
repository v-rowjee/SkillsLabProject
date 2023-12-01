using SkillsLabProject.BLL;
using SkillsLabProject.DAL.Common;
using SkillsLabProject.Models;
using SkillsLabProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL
{
    public interface IPreRequisiteDAL : IDAL<PreRequisiteModel>
    {

    }
    public class PreRequisiteDAL : IPreRequisiteDAL
    {
        private const string AddPreRequisiteQuery = @"
            INSERT [dbo].[PreRequisite] (Detail) VALUES (@Detail);
        ";
        private const string GetAllPreRequisitesQuery = @"
            SELECT PreRequisiteId, Detail
            FROM [dbo].[PreRequisite]
        ";
        private const string GetPreRequisiteQuery = @"
            SELECT PreRequisiteId, Detail
            FROM [dbo].[PreRequisite]
            WHERE [PreRequisiteId] = @PreRequisiteId
        ";
        private const string UpdatePreRequisiteQuery = @"
            UPDATE [dbo].[PreRequisite]
            SET Detail=@Detail
            WHERE PreRequisiteId=@PreRequisiteId;
        ";
        private const string DeletePreRequisiteQuery = @"
            DELETE FROM [dbo].[TrainingPreRequisite] WHERE PreRequisiteId=@PreRequisiteId;
            DELETE FROM [dbo].[PreRequisite] WHERE PreRequisiteId=@PreRequisiteId
        ";

        public bool Add(PreRequisiteModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Detail", model.Detail));

            var preRequisiteInserted = DBCommand.InsertUpdateData(AddPreRequisiteQuery, parameters);

            return preRequisiteInserted;
        }

        public bool Delete(int PreRequisiteId)
        {
            var parameter = new SqlParameter("@PreRequisiteId", PreRequisiteId);
            return DBCommand.DeleteData(DeletePreRequisiteQuery, parameter);
        }

        public IEnumerable<PreRequisiteModel> GetAll()
        {
            var PreRequisites = new List<PreRequisiteModel>();
            PreRequisiteModel PreRequisite;

            var dt = DBCommand.GetData(GetAllPreRequisitesQuery);
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
            var PreRequisite = new PreRequisiteModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@PreRequisiteId", PreRequisiteId));

            var dt = DBCommand.GetDataWithCondition(GetPreRequisiteQuery, parameters);
            foreach (DataRow row in dt.Rows)
            {
                PreRequisite.PreRequisiteId = int.Parse(row["PreRequisiteId"].ToString());
                PreRequisite.Detail = row["Title"].ToString();
            }
            return PreRequisite;
        }

        public bool Update(PreRequisiteModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@PreRequisiteId", model.PreRequisiteId));
            parameters.Add(new SqlParameter("@Detail", model.Detail));

            var PreRequisiteUpdated = DBCommand.InsertUpdateData(UpdatePreRequisiteQuery, parameters);

            return PreRequisiteUpdated;
        }
    }
}
