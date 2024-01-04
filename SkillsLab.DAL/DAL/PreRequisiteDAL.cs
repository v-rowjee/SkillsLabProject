using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface IPreRequisiteDAL : IDAL<PreRequisiteModel>
    {
    }
    public class PreRequisiteDAL : IPreRequisiteDAL
    {
        public async Task<bool> AddAsync(PreRequisiteModel model)
        {
            const string AddPreRequisiteQuery = @"
                INSERT [dbo].[PreRequisite] (Detail) VALUES (@Detail);
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Detail", model.Detail)
            };

            return await DBCommand.InsertDataAsync(AddPreRequisiteQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(int preRequisiteId)
        {
            const string DeletePreRequisiteQuery = @"
                DELETE FROM [dbo].[TrainingPreRequisite] WHERE PreRequisiteId=@PreRequisiteId;
                DELETE FROM [dbo].[PreRequisite] WHERE PreRequisiteId=@PreRequisiteId
            ";
            var parameter = new SqlParameter("@PreRequisiteId", preRequisiteId);

            return await DBCommand.DeleteDataAsync(DeletePreRequisiteQuery, parameter).ConfigureAwait(false);
        }

        public async Task<IEnumerable<PreRequisiteModel>> GetAllAsync()
        {
            const string GetAllPreRequisitesQuery = @"
                SELECT p.PreRequisiteId, p.Detail, t.TrainingId
                FROM [dbo].[PreRequisite] p
                INNER JOIN [dbo].[TrainingPreRequisite] t
                ON p.PreRequisiteId = t.PreRequisiteId
            ";

            var preRequisites = new List<PreRequisiteModel>();

            using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllPreRequisitesQuery).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    var preRequisite = new PreRequisiteModel
                    {
                        PreRequisiteId = dataReader.GetInt32(dataReader.GetOrdinal("PreRequisiteId")),
                        Detail = dataReader.GetString(dataReader.GetOrdinal("Detail")),
                        TrainingId = dataReader.GetInt32(dataReader.GetOrdinal("TrainingId"))
                    };

                    preRequisites.Add(preRequisite);
                }
            }

            return preRequisites;
        }

        public async Task<PreRequisiteModel> GetByIdAsync(int preRequisiteId)
        {
            const string GetPreRequisiteQuery = @"
                SELECT p.PreRequisiteId, p.Detail, t.TrainingId
                FROM [dbo].[PreRequisite] p
                INNER JOIN [dbo].[TrainingPreRequisite] t
                ON p.PreRequisiteId = t.PreRequisiteId
                WHERE [PreRequisiteId] = @PreRequisiteId
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@PreRequisiteId", preRequisiteId)
            };

            var preRequisite = new PreRequisiteModel();

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetPreRequisiteQuery, parameters).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync().ConfigureAwait(false))
                {
                    preRequisite.PreRequisiteId = dataReader.GetInt32(dataReader.GetOrdinal("PreRequisiteId"));
                    preRequisite.Detail = dataReader.GetString(dataReader.GetOrdinal("Detail"));
                    preRequisite.TrainingId = dataReader.GetInt32(dataReader.GetOrdinal("TrainingId"));
                }
            }
            return preRequisite;
        }

        public async Task<bool> UpdateAsync(PreRequisiteModel model)
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

            return await DBCommand.UpdateDataAsync(UpdatePreRequisiteQuery, parameters).ConfigureAwait(false);
        }

    }
}