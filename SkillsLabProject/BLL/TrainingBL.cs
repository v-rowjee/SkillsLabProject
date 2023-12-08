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
        bool AddTraining(TrainingViewModel model);
        bool UpdateTraining(TrainingViewModel model);
        bool DeleteTraining(int trainingId);
    }
    public class TrainingBL : ITrainingBL
    {
        private readonly ITrainingDAL _trainingDAL;
        private readonly IDepartmentDAL _departmentDAL;

        public TrainingBL(ITrainingDAL trainingDAL, IDepartmentDAL departmentDAL)
        {
            _trainingDAL = trainingDAL;
            _departmentDAL = departmentDAL;
        }

        public bool AddTraining(TrainingViewModel training)
        {
            if(training.PreRequisites == null)
            {
                var trainingModel = new TrainingModel
                {
                    Title = training.Title,
                    Description = training.Description,
                    Deadline = training.Deadline,
                    Capacity = training.Capacity,
                    PriorityDepartment = _departmentDAL.GetById((int)training.DepartmentId)
                };
                return _trainingDAL.Add(trainingModel);
            }
            else
            {
                return _trainingDAL.Add(training);
            }
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
        public bool UpdateTraining(TrainingViewModel training)
        {
            if (training.PreRequisites == null)
            {
                var trainingModel = new TrainingModel
                {
                    Title = training.Title,
                    Description = training.Description,
                    Deadline = training.Deadline,
                    Capacity = training.Capacity,
                    PriorityDepartment = _departmentDAL.GetById((int)training.DepartmentId)
                };
                return _trainingDAL.Update(trainingModel);
            }
            else
            {
                return _trainingDAL.Update(training);
            }
        }
    }
}
