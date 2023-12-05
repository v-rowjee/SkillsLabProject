using SkillsLabProject.Models;
using SkillsLabProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsLabProject.DAL.RepositoryDAL;
using SkillsLabProject.DAL.Models;

namespace SkillsLabProject.BLL
{
    public interface ITrainingBL
    {
        IEnumerable<Training> GetAllTrainings();
        Training GetTrainingById(int trainingId);
        bool AddTraining(Training model);
        bool UpdateTraining(Training model);
        bool DeleteTraining(int trainingId);
    }
    public class TrainingBL
    {
        private readonly IRepositoryDAL<TrainingBL> _trainingDAL;

        public TrainingBL(IRepositoryDAL<TrainingBL> trainingDAL)
        {
            _trainingDAL = trainingDAL;
        }

    }
}
