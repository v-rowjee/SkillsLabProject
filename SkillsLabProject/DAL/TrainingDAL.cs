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
            var trainings = new List<TrainingModel>();
            TrainingModel training;
            const string GetAllTrainingsQuery = @"
                SELECT TrainingId, Title, Deadline, Capacity, PriorityDepartmentId 
                FROM [dbo].[Training]
            ";
            var dt = DBCommand.GetData(GetAllTrainingsQuery);
            foreach (DataRow row in dt.Rows)
            {
                training = new TrainingModel();
                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.Title = row["Title"].ToString();
                training.Deadline = DateTime.Parse(row["Datetime"].ToString());
                training.Capacity = int.Parse(row["Capacity"].ToString());
                training.PriorityDepartmentId = int.Parse(row["PriorityDepartmentId"].ToString());

                trainings.Add(training);
            }
            return trainings;
        }

        public TrainingModel GetById(int trainingId)
        {
            var training = new TrainingModel();
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TrainingId", trainingId));
            const string GetTrainingQuery = @"
                SELECT TrainingId, Title, Deadline, Capacity
                FROM [dbo].[Training]
                WHERE [TrainingId] = @TrainingId
            ";
            var dt = DBCommand.GetDataWithCondition(GetTrainingQuery, parameters);
            foreach(DataRow row in dt.Rows)
            {
                training.TrainingId = int.Parse(row["TrainingId"].ToString());
                training.Title = row["Title"].ToString();
                training.Deadline = DateTime.Parse(row["Datetime"].ToString());
                training.Capacity = int.Parse(row["Capacity"].ToString());
                training.PriorityDepartmentId = int.Parse(row["PriorityDepartmentId"].ToString());
            }
            return training;
        }

        public bool Add(TrainingModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Title", model.Title));
            parameters.Add(new SqlParameter("@Deadline", model.Deadline));
            parameters.Add(new SqlParameter("@Capacity", model.Capacity));
            parameters.Add(new SqlParameter("@PriorityDepartmentId", model.PriorityDepartmentId));
            const string AddTrainingQuery = @"
                INSERT [dbo].[Training] (Title, Deadline, Capacity, PriorityDepartmentId) VALUES (@Title, @Deadline, @Capacity, @PriorityDepartmentId);
            ";
            var trainingInserted = DBCommand.InsertUpdateData(AddTrainingQuery, parameters);

            return trainingInserted;

        }

        public bool Update(TrainingModel model)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@TrainingId", model.TrainingId));
            parameters.Add(new SqlParameter("@Title", model.Title));
            parameters.Add(new SqlParameter("@Deadline",model.Deadline));
            parameters.Add(new SqlParameter("@Capacity", model.Capacity));
            parameters.Add(new SqlParameter("@PriorityDepartmentId", model.PriorityDepartmentId));
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
