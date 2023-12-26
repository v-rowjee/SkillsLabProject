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
        IEnumerable<TrainingModel> GetAllTrainings();
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

        public TrainingBL(ITrainingDAL trainingDAL, IDepartmentDAL departmentDAL,IEnrollmentDAL enrollmentDAL, IEmployeeDAL employeeDAL)
        {
            _trainingDAL = trainingDAL;
            _departmentDAL = departmentDAL;
            _enrollmentDAL = enrollmentDAL;
            _employeeDAL = employeeDAL;
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

        public bool CloseTraining(int trainingId)
        {
            var training = _trainingDAL.GetById(trainingId);

            var enrollments = _enrollmentDAL
                .GetAll()
                .Where(e => e.TrainingId == trainingId)
                .OrderByDescending(e => e.UpdatedOn)
                .Take(training.Capacity)
                .ToList();

            foreach (var enrollment in enrollments)
            {
                var employee = _employeeDAL.GetEmployeeById(enrollment.EmployeeId);

                if (training.PriorityDepartment.DepartmentId == employee.Department.DepartmentId)
                {
                    enrollment.Status = Status.Approved;
                }
                else
                {
                    enrollment.Status = Status.Declined;
                }

                _enrollmentDAL.Update(enrollment);
            }
            training.IsClosed = true;
            return _trainingDAL.Update(training);
        }

    }
}
