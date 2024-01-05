using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.DAL
{
    public interface ITrainingDAL : IDAL<TrainingModel>
    {
        Task<bool> AddAsync(TrainingViewModel training);
        Task<bool> UpdateAsync(TrainingViewModel training);
        Task<bool> CloseAsync(int trainingId);
        Task<bool> AutoCloseAsync();
    }
    public class TrainingDAL : ITrainingDAL
    {
        public async Task<IEnumerable<TrainingModel>> GetAllAsync()
        {
            const string GetAllTrainingsQuery = @"
                SELECT t.TrainingId, t.Title, t.Description, t.Deadline, t.Capacity, t.PriorityDepartmentId, d.Title as DepartmentTitle, t.IsClosed
                FROM [dbo].[Training] t 
                LEFT JOIN [dbo].[Department] d
                ON t.PriorityDepartmentId = d.DepartmentId
                WHERE t.IsActive = 1
            ";

            var trainings = new List<TrainingModel>();

            using (SqlDataReader dataReader = await DBCommand.GetDataAsync(GetAllTrainingsQuery).ConfigureAwait(false))
            {
                while (await dataReader.ReadAsync())
                {
                    var training = new TrainingModel
                    {
                        TrainingId = dataReader.GetInt32(dataReader.GetOrdinal("TrainingId")),
                        Title = dataReader.GetString(dataReader.GetOrdinal("Title")),
                        Description = dataReader.GetString(dataReader.GetOrdinal("Description")),
                        Deadline = dataReader.GetDateTime(dataReader.GetOrdinal("Deadline")),
                        Capacity = dataReader.GetInt32(dataReader.GetOrdinal("Capacity")),
                        IsClosed = dataReader.GetBoolean(dataReader.GetOrdinal("IsClosed"))
                    };

                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("PriorityDepartmentId")))
                    {
                        training.PriorityDepartment = new DepartmentModel
                        {
                            DepartmentId = dataReader.GetInt32(dataReader.GetOrdinal("PriorityDepartmentId")),
                            Title = dataReader.GetString(dataReader.GetOrdinal("DepartmentTitle"))
                        };
                    }
                    else
                    {
                        training.PriorityDepartment = null;
                    }

                    trainings.Add(training);
                }
            }

            return trainings;
        }

        public async Task<TrainingModel> GetByIdAsync(int trainingId)
        {
            const string GetTrainingQuery = @"
                SELECT t.TrainingId, t.Title, t.Description, t.Deadline, t.Capacity, t.PriorityDepartmentId, d.Title as DepartmentTitle, t.IsClosed
                FROM [dbo].[Training] t 
                LEFT JOIN Department d
                ON t.PriorityDepartmentId = d.DepartmentId
                WHERE [TrainingId] = @TrainingId
                AND t.IsActive = 1
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", trainingId)
            };

            using (SqlDataReader dataReader = await DBCommand.GetDataWithConditionAsync(GetTrainingQuery, parameters).ConfigureAwait(false))
            {
                var training = new TrainingModel();

                if (await dataReader.ReadAsync())
                {
                    training.TrainingId = dataReader.GetInt32(dataReader.GetOrdinal("TrainingId"));
                    training.Title = dataReader.GetString(dataReader.GetOrdinal("Title"));
                    training.Description = dataReader.GetString(dataReader.GetOrdinal("Description"));
                    training.Deadline = dataReader.GetDateTime(dataReader.GetOrdinal("Deadline"));
                    training.Capacity = dataReader.GetInt32(dataReader.GetOrdinal("Capacity"));
                    training.IsClosed = dataReader.GetBoolean(dataReader.GetOrdinal("IsClosed"));

                    if (!dataReader.IsDBNull(dataReader.GetOrdinal("PriorityDepartmentId")))
                    {
                        training.PriorityDepartment = new DepartmentModel
                        {
                            DepartmentId = dataReader.GetInt32(dataReader.GetOrdinal("PriorityDepartmentId")),
                            Title = dataReader.GetString(dataReader.GetOrdinal("DepartmentTitle"))
                        };
                    }
                    else
                    {
                        training.PriorityDepartment = null;
                    }
                }

                return training;
            }
        }

        public async Task<bool> AddAsync(TrainingViewModel training)
        {
            const string AddTrainingQuery = @"
                BEGIN TRANSACTION
                    INSERT [dbo].[Training] (Title, Description, Deadline, Capacity, PriorityDepartmentId) 
                    VALUES (@Title, @Description, @Deadline, @Capacity, @PriorityDepartmentId);

                    DECLARE @TrainingId INT = @@IDENTITY

                    DECLARE @Details TABLE (Detail NVARCHAR(MAX))
                    INSERT INTO @Details
                    SELECT value FROM STRING_SPLIT(@Prerequisites, ',')

                    INSERT INTO PreRequisite (Detail)
                    SELECT Detail
                    FROM @Details d
                    WHERE NOT EXISTS (SELECT 1 FROM PreRequisite WHERE Detail = d.Detail)

                    INSERT INTO TrainingPreRequisite (PreRequisiteId, TrainingId)
                    SELECT p.PreRequisiteId, @TrainingId
                    FROM PreRequisite p
                    JOIN @Details d ON p.Detail = d.Detail
                COMMIT
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Description", training.Description),
                new SqlParameter("@Deadline", training.Deadline),
                new SqlParameter("@Capacity", training.Capacity),
                new SqlParameter("@Prerequisites", string.Join(",", training.PreRequisites))
            };

            if (training.PriorityDepartment == null)
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", training.PriorityDepartment.DepartmentId));
            }

            return await DBCommand.InsertDataAsync(AddTrainingQuery, parameters).ConfigureAwait(false);
        }


        public async Task<bool> AddAsync(TrainingModel training)
        {
            const string AddTrainingQuery = @"
                INSERT [dbo].[Training] (Title,Description, Deadline, Capacity, PriorityDepartmentId) 
                VALUES (@Title,@Description, @Deadline, @Capacity, @PriorityDepartmentId);
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Description", training.Description),
                new SqlParameter("@Deadline", training.Deadline),
                new SqlParameter("@Capacity", training.Capacity),
            };

            if (training.PriorityDepartment == null)
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", training.PriorityDepartment.DepartmentId));
            }

            return await DBCommand.InsertDataAsync(AddTrainingQuery, parameters).ConfigureAwait(false);
        }


        public async Task<bool> UpdateAsync(TrainingViewModel training)
        {
            const string UpdateTrainingQuery = @"
                BEGIN TRANSACTION
                    UPDATE [dbo].[Training]
                    SET Title=@Title, Description=@Description, Deadline=@Deadline, Capacity=@Capacity, PriorityDepartmentId=@PriorityDepartmentId
                    WHERE TrainingId=@TrainingId;

                    DECLARE @Details TABLE (Detail NVARCHAR(MAX))
                    INSERT INTO @Details
                    SELECT value FROM STRING_SPLIT(@Prerequisites, ',')

                    MERGE INTO PreRequisite AS target
                    USING @Details AS source ON target.Detail = source.Detail
                    WHEN NOT MATCHED THEN
                    INSERT (Detail) VALUES (source.Detail);

                    DELETE FROM TrainingPreRequisite WHERE TrainingId = @TrainingId;

                    INSERT INTO TrainingPreRequisite (PreRequisiteId, TrainingId)
                    SELECT p.PreRequisiteId, @TrainingId
                    FROM PreRequisite p
                    JOIN @Details d ON p.Detail = d.Detail

                    DELETE Proof
                    FROM Proof p
                    INNER JOIN Enrollment e ON p.EnrollmentId = e.EnrollmentId
                    WHERE e.TrainingId = @TrainingId;

                    DELETE FROM Enrollment WHERE TrainingId = @TrainingId
                COMMIT
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", training.TrainingId),
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Description", training.Description),
                new SqlParameter("@Deadline", training.Deadline),
                new SqlParameter("@Capacity", training.Capacity),
                new SqlParameter("@Prerequisites", string.Join(",", training.PreRequisites))
            };

            if (training.PriorityDepartment == null)
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", training.PriorityDepartment.DepartmentId));
            }

            return await DBCommand.UpdateDataAsync(UpdateTrainingQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> UpdateAsync(TrainingModel training)
        {
            const string UpdateTrainingQuery = @"
                UPDATE [dbo].[Training]
                SET Title=@Title, Description=@Description, Deadline=@Deadline, Capacity=@Capacity, PriorityDepartmentId=@PriorityDepartmentId, IsClosed=@IsClosed
                WHERE TrainingId=@TrainingId;
            ";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", training.TrainingId),
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Description", training.Description),
                new SqlParameter("@Deadline", training.Deadline),
                new SqlParameter("@Capacity", training.Capacity),
                new SqlParameter("@IsClosed", training.IsClosed ? 1 : 0),
            };

            if (training.PriorityDepartment == null)
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", training.PriorityDepartment.DepartmentId));
            }

            return await DBCommand.UpdateDataAsync(UpdateTrainingQuery, parameters).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(int trainingId)
        {
            const string DeleteTrainingQuery = @"
                UPDATE Training
                SET IsActive=0
                WHERE TrainingId=@TrainingId
            ";

            var parameter = new SqlParameter("@TrainingId", trainingId);
            return await DBCommand.DeleteDataAsync(DeleteTrainingQuery, parameter).ConfigureAwait(false);
        }


        public async Task<bool> CloseAsync(int trainingId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", trainingId),
            };

            return await DBCommand.ExecuteStoredProcedureAsync("dbo.ProcessEnrollments", parameters).ConfigureAwait(false);
        }

        public async Task<bool> AutoCloseAsync()
        {
            return await DBCommand.ExecuteStoredProcedureAsync("dbo.AutoProcessEnrollments").ConfigureAwait(false);
        }
    }
}
