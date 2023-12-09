using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Common.DAL;
using SkillsLabProject.Common.Models;

namespace SkillsLabProject.DAL.DAL
{
    public interface ITrainingDAL : IDAL<TrainingModel>
    {
        bool Add(TrainingViewModel training);
        bool Update(TrainingViewModel training);
    }
    public class TrainingDAL : ITrainingDAL
    {
        public IEnumerable<TrainingModel> GetAll()
        {
            const string GetAllTrainingsQuery = @"
                SELECT t.TrainingId, t.Title, t.Description, t.Deadline, t.Capacity, t.PriorityDepartmentId, d.Title as DepartmentTitle
                FROM [dbo].[Training] t 
                LEFT JOIN [dbo].[Department] d
                ON t.PriorityDepartmentId = d.DepartmentId
            ";
            var dt = DBCommand.GetData(GetAllTrainingsQuery);
            TrainingModel training;
            var trainings = new List<TrainingModel>();
            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel
                {
                    TrainingId = (int)row["TrainingId"],
                    Title = row["Title"].ToString(),
                    Description = row["Description"].ToString(),
                    Deadline = DateTime.Parse(row["Deadline"].ToString()),
                    Capacity = (int)row["Capacity"]
                };
                if (row["PriorityDepartmentId"] is DBNull)
                {
                    training.PriorityDepartment = null;
                }
                else 
                {
                    training.PriorityDepartment = new DepartmentModel
                    {
                        DepartmentId = (int) row["PriorityDepartmentId"],
                        Title = row["DepartmentTitle"].ToString(),
                    };
                }
                trainings.Add(training);
            }
            return trainings;
        }
        public TrainingModel GetById(int trainingId)
        {
            const string GetTrainingQuery = @"
                SELECT t.TrainingId, t.Title, t.Description, t.Deadline, t.Capacity, t.PriorityDepartmentId, d.Title as DepartmentTitle
                FROM [dbo].[Training] t 
                LEFT JOIN Department d
                ON t.PriorityDepartmentId = d.DepartmentId
                WHERE [TrainingId] = @TrainingId
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", trainingId)
            };
            var dt = DBCommand.GetDataWithCondition(GetTrainingQuery, parameters);
            var training = new TrainingModel();
            foreach (DataRow row in dt.Rows)
            {
                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.Title = row["Title"].ToString();
                training.Description = row["Description"].ToString();
                training.Deadline = DateTime.Parse(row["Deadline"].ToString());
                training.Capacity = int.Parse(row["Capacity"].ToString());
                if (row["PriorityDepartmentId"] is DBNull)
                {
                    training.PriorityDepartment = null;
                }
                else
                {
                    training.PriorityDepartment = new DepartmentModel
                    {
                        DepartmentId = (int)row["PriorityDepartmentId"],
                        Title = row["DepartmentTitle"].ToString(),
                    };
                }
            }
            return training;
        }
        public bool Add(TrainingViewModel training)
        {
            const string AddTrainingQuery = @"
                BEGIN TRANSACTION
                    INSERT [dbo].[Training] (Title,Description, Deadline, Capacity, PriorityDepartmentId) 
                    VALUES (@Title,@Description, @Deadline, @Capacity, @PriorityDepartmentId);

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
                new SqlParameter("@Prerequisites",string.Join(",", training.PreRequisites))
            };
            if(training.DepartmentId == null)
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", training.DepartmentId));
            }

            return DBCommand.InsertUpdateData(AddTrainingQuery, parameters);
        }

        public bool Add(TrainingModel training)
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
            return DBCommand.InsertUpdateData(AddTrainingQuery, parameters);
        }

        public bool Update(TrainingViewModel training)
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
                COMMIT
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", training.TrainingId),
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Description",training.Description),
                new SqlParameter("@Deadline", training.Deadline),
                new SqlParameter("@Capacity", training.Capacity),
                new SqlParameter("@Prerequisites",string.Join(",", training.PreRequisites))
            };
            if (training.DepartmentId == null)
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter("@PriorityDepartmentId", training.DepartmentId));
            }
            return DBCommand.InsertUpdateData(UpdateTrainingQuery, parameters);
        }
        public bool Update(TrainingModel training)
        {
            const string UpdateTrainingQuery = @"
                UPDATE [dbo].[Training]
                SET Title=@Title, Description=@Description, Deadline=@Deadline, Capacity=@Capacity, PriorityDepartmentId=@PriorityDepartmentId
                WHERE TrainingId=@TrainingId;
            ";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", training.TrainingId),
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Description",training.Description),
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
            return DBCommand.InsertUpdateData(UpdateTrainingQuery, parameters);
        }
        public bool Delete(int trainingId)
        {
            const string DeleteTrainingQuery = @"
                BEGIN TRANSACTION
                    DELETE FROM [dbo].[TrainingPreRequisite] WHERE TrainingId=@TrainingId;
                    DELETE FROM [dbo].[Enrollment] WHERE TrainingId=@TrainingId;
                    DELETE FROM [dbo].[Training] WHERE TrainingId=@TrainingId
                COMMIT
            ";
            var parameter = new SqlParameter("@TrainingId", trainingId);
            return DBCommand.DeleteData(DeleteTrainingQuery, parameter);
        }
    }
}
