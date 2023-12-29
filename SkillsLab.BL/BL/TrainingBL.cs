using SkillsLabProject.DAL.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.Common.Models;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Common.Enums;

namespace SkillsLabProject.BL.BL
{
    public interface ITrainingBL
    {
        IEnumerable<TrainingViewModel> GetAllTrainings();
        TrainingModel GetTrainingById(int trainingId);
        bool AddTraining(TrainingViewModel model);
        bool UpdateTraining(TrainingViewModel model);
        bool DeleteTraining(int trainingId);
        bool CloseTraining(int trainingId);
    }
    public class TrainingBL : ITrainingBL
    {
        private readonly ITrainingDAL _trainingDAL;
        private readonly IDepartmentDAL _departmentDAL;
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly IEmployeeDAL _employeeDAL;
        private readonly IPreRequisiteDAL _preRequisiteDAL;

        public TrainingBL(ITrainingDAL trainingDAL, IDepartmentDAL departmentDAL,IEnrollmentDAL enrollmentDAL, IEmployeeDAL employeeDAL, IPreRequisiteDAL preRequisiteDAL)
        {
            _trainingDAL = trainingDAL;
            _departmentDAL = departmentDAL;
            _enrollmentDAL = enrollmentDAL;
            _employeeDAL = employeeDAL;
            _preRequisiteDAL = preRequisiteDAL;
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
                    PriorityDepartment = _departmentDAL.GetById(training.PriorityDepartment.DepartmentId)
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
        public IEnumerable<TrainingViewModel> GetAllTrainings()
        {
            List<TrainingViewModel> trainingModels = new List<TrainingViewModel>();
            var trainings = _trainingDAL.GetAll();
            foreach (var training in trainings)
            {
                var employeeEnrolled = _enrollmentDAL.GetAll().Where(e => e.TrainingId == training.TrainingId).Count();
                var prerequisitesString = _preRequisiteDAL.GetAll().Where(p => p.TrainingId == training.TrainingId).Select(p => p.Detail).ToList();
                var trainingModel = new TrainingViewModel
                {
                    TrainingId = training.TrainingId,
                    Title = training.Title,
                    Description = training.Description,
                    Deadline = training.Deadline,
                    Capacity = training.Capacity,
                    PreRequisites = prerequisitesString,
                    IsClosed = training.IsClosed,
                    PriorityDepartment = training.PriorityDepartment,
                    SeatsLeft = training.Capacity - employeeEnrolled,
                };
                trainingModels.Add(trainingModel);
            }
            return trainingModels;
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
                    PriorityDepartment = _departmentDAL.GetById(training.PriorityDepartment.DepartmentId)
                };
                return _trainingDAL.Update(trainingModel);
            }
            else
            {
                return _trainingDAL.Update(training);
            }
        }

        public bool CloseTraining(int trainingId)
        {
            return _trainingDAL.Close(trainingId);
        }

    }
}
