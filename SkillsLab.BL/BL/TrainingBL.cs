using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkillsLabProject.Common.Models.ViewModels;
using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;
using System.Diagnostics;
using SkillsLabProject.Common.Custom;
using SkillsLabProject.Common.Enums;

namespace SkillsLabProject.BL.BL
{
    public interface ITrainingBL
    {
        Task<IEnumerable<TrainingViewModel>> GetAllTrainingsAsync();
        Task<TrainingModel> GetTrainingByIdAsync(int trainingId);
        Task<bool> AddTrainingAsync(TrainingViewModel model);
        Task<bool> UpdateTrainingAsync(TrainingViewModel model);
        Task<Result> DeleteTrainingAsync(int trainingId);
        Task<bool> CloseTrainingAsync(int trainingId);
        Task<bool> AutoCloseTrainingAsync();
    }

    public class TrainingBL : ITrainingBL
    {
        private readonly ITrainingDAL _trainingDAL;
        private readonly IDepartmentDAL _departmentDAL;
        private readonly IEnrollmentDAL _enrollmentDAL;
        private readonly IPreRequisiteDAL _preRequisiteDAL;

        public TrainingBL(ITrainingDAL trainingDAL, IDepartmentDAL departmentDAL, IEnrollmentDAL enrollmentDAL, IPreRequisiteDAL preRequisiteDAL)
        {
            _trainingDAL = trainingDAL;
            _departmentDAL = departmentDAL;
            _enrollmentDAL = enrollmentDAL;
            _preRequisiteDAL = preRequisiteDAL;
        }

        public async Task<bool> AddTrainingAsync(TrainingViewModel training)
        {
            if (training.PreRequisites == null)
            {
                var trainingModel = new TrainingModel
                {
                    Title = training.Title,
                    Description = training.Description,
                    Deadline = training.Deadline,
                    Capacity = training.Capacity,
                };
                if (training.PriorityDepartment != null)
                {
                    trainingModel.PriorityDepartment = await _departmentDAL.GetByIdAsync(training.PriorityDepartment.DepartmentId);
                }
                return await _trainingDAL.AddAsync(trainingModel);
            }
            else
            {
                return await _trainingDAL.AddAsync(training);
            }
        }

        public async Task<Result> DeleteTrainingAsync(int trainingId)
        {
            var enrollments = (await _enrollmentDAL.GetAllAsync()).Where(e => e.TrainingId == trainingId && e.Status == Status.Approved);
            if (enrollments.Any())
            {
                return new Result()
                {
                    IsSuccess = false,
                    Message = $"Cannot delete training because {enrollments.Count()} enrollments was approved."
                };
            }

            var trainingDeleted = await _trainingDAL.DeleteAsync(trainingId);

            return new Result()
            {
                IsSuccess = trainingDeleted,
                Message = trainingDeleted ? "Training removed." : "An error occurred while trying to delete training."
            };
        }


        public async Task<TrainingModel> GetTrainingByIdAsync(int trainingId)
        {
            return await _trainingDAL.GetByIdAsync(trainingId);
        }

        public async Task<IEnumerable<TrainingViewModel>> GetAllTrainingsAsync()
        {
            List<TrainingViewModel> trainingModels = new List<TrainingViewModel>();
            var trainings = await _trainingDAL.GetAllAsync();
            foreach (var training in trainings)
            {
                var employeeEnrolled = (await _enrollmentDAL.GetAllAsync()).Count(e => e.TrainingId == training.TrainingId && e.Status == Common.Enums.Status.Approved);
                var prerequisitesString = (await _preRequisiteDAL.GetAllAsync()).Where(p => p.TrainingId == training.TrainingId).Select(p => p.Detail).ToList();
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

        public async Task<bool> UpdateTrainingAsync(TrainingViewModel training)
        {
            if (training.PreRequisites == null)
            {
                var trainingModel = new TrainingModel
                {
                    Title = training.Title,
                    Description = training.Description,
                    Deadline = training.Deadline,
                    Capacity = training.Capacity,
                    PriorityDepartment = await _departmentDAL.GetByIdAsync(training.PriorityDepartment.DepartmentId)
                };
                return await _trainingDAL.UpdateAsync(trainingModel);
            }
            else
            {
                return await _trainingDAL.UpdateAsync(training);
            }
        }

        public async Task<bool> CloseTrainingAsync(int trainingId)
        {
            return await _trainingDAL.CloseAsync(trainingId);
        }

        public async Task<bool> AutoCloseTrainingAsync()
        {
            Debug.WriteLine("Job Executed");
            return await _trainingDAL.AutoCloseAsync();
        }
    }
}