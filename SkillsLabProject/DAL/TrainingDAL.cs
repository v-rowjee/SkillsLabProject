using SkillsLabProject.Models;
using SkillsLabProject.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SkillsLabProject.BLL;
using System.Data.SqlClient;
using System.Reflection;

namespace SkillsLabProject.DAL
{
    public interface ITrainingDAL : IDAL<TrainingModel>
    {
    }
    public class TrainingDAL : ITrainingDAL
    {
        public IEnumerable<TrainingModel> GetAll()
        {
            const string GetAllTrainingsQuery = @"
                SELECT TrainingId, Title, Deadline, Capacity, PriorityDepartmentId 
                FROM [dbo].[Training]
            ";
            var dt = DBCommand.GetData(GetAllTrainingsQuery);
            TrainingModel training;
            var trainings = new List<TrainingModel>();
            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel
                {
                    TrainingId = int.Parse(row["TrainingId"].ToString()),
                    Title = row["Title"].ToString(),
                    Deadline = DateTime.Parse(row["Deadline"].ToString()),
                    Capacity = int.Parse(row["Capacity"].ToString()),
                    PriorityDepartmentId = int.Parse(row["PriorityDepartmentId"].ToString())
                };
                trainings.Add(training);
            }
            return trainings;
        }

        public TrainingModel GetById(int trainingId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", trainingId)
            };
            const string GetTrainingQuery = @"
                SELECT TrainingId, Title, Deadline, Capacity
                FROM [dbo].[Training]
                WHERE [TrainingId] = @TrainingId
            ";
            var dt = DBCommand.GetDataWithCondition(GetTrainingQuery, parameters);
            var training = new TrainingModel();
            foreach (DataRow row in dt.Rows)
            {
                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.Title = row["Title"].ToString();
                training.Deadline = DateTime.Parse(row["Datetime"].ToString());
                training.Capacity = int.Parse(row["Capacity"].ToString());
                training.PriorityDepartmentId = int.Parse(row["PriorityDepartmentId"]?.ToString());
            }
            return training;
        }

        public bool Add(TrainingModel training)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Deadline", training.Deadline),
                new SqlParameter("@Capacity", training.Capacity),
                new SqlParameter("@PriorityDepartmentId", training.PriorityDepartmentId)
            };
            const string AddTrainingQuery = @"
                INSERT [dbo].[Training] (Title, Deadline, Capacity, PriorityDepartmentId) 
                VALUES (@Title, @Deadline, @Capacity, @PriorityDepartmentId);
            ";
            return DBCommand.InsertUpdateData(AddTrainingQuery, parameters);
        }
        public bool Update(TrainingModel training)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@TrainingId", training.TrainingId),
                new SqlParameter("@Title", training.Title),
                new SqlParameter("@Deadline", training.Deadline),
                new SqlParameter("@Capacity", training.Capacity),
                new SqlParameter("@PriorityDepartmentId", training.PriorityDepartmentId)
            };
            const string UpdateTrainingQuery = @"
                UPDATE [dbo].[Training]
                SET Title=@Title, Deadline=@Deadline, Capacity=@Capacity, PriorityDepartment=@PriorityDepartment
                WHERE TrainingId=@TrainingId;
            ";
            return DBCommand.InsertUpdateData(UpdateTrainingQuery, parameters);
        }
        public bool Delete(int trainingId)
        {
            var parameter = new SqlParameter("@TrainingId", trainingId);
            const string DeleteTrainingQuery = @"
                BEGIN TRANSACTION
                    DELETE FROM [dbo].[TrainingPreRequisite] WHERE TrainingId=@TrainingId;
                    DELETE FROM [dbo].[Enrollment] WHERE TrainingId=@TrainingId;
                    DELETE FROM [dbo].[Training] WHERE TrainingId=@TrainingId
                COMMIT
            ";
            return DBCommand.DeleteData(DeleteTrainingQuery, parameter);
        }
    }
}
