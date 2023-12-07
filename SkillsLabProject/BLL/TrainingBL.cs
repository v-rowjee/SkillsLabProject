using SkillsLabProject.Models;
using SkillsLabProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.DAL.Common;
using SkillsLabProject.Models.ViewModels;

namespace SkillsLabProject.BLL
{
    public interface ITrainingBL
    {
        IEnumerable<TrainingModel> GetAllTrainings();
        TrainingModel GetTrainingById(int trainingId);
        bool AddTraining(TrainingModel model);
        bool UpdateTraining(TrainingModel model);
        bool DeleteTraining(int trainingId);
    }
    public class TrainingBL : ITrainingBL
    {
        private readonly ITrainingDAL _trainingDAL;

        public TrainingBL(ITrainingDAL trainingDAL)
        {
            _trainingDAL = trainingDAL;
        }

        public bool AddTraining(TrainingModel training)
        {
            return _trainingDAL.Add(training);
        }
        public bool DeleteTraining(int trainingId)
        {
            return _trainingDAL.Delete(trainingId);
        }
        public TrainingModel GetTrainingById(int trainingId)
        {
            return _trainingDAL.GetById(trainingId);
        }
        public IEnumerable<TrainingModel> GetAllTrainings()
        {
            return _trainingDAL.GetAll();
        }
        public bool UpdateTraining(TrainingModel training)
        {
            return _trainingDAL.Update(training);
        }
    }
}
