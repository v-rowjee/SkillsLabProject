using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsLabProject.BL.BL
{
    public interface IPreRequisiteBL
    {
        Task<IEnumerable<PreRequisiteModel>> GetAllPreRequisitesAsync();
        Task<PreRequisiteModel> GetPreRequisiteByIdAsync(int preRequisiteId);
        Task<bool> AddPreRequisiteAsync(PreRequisiteModel model);
        Task<bool> UpdatePreRequisiteAsync(PreRequisiteModel model);
        Task<bool> DeletePreRequisiteAsync(int preRequisiteId);
    }

    public class PreRequisiteBL : IPreRequisiteBL
    {
        private readonly IPreRequisiteDAL _preRequisiteDAL;

        public PreRequisiteBL(IPreRequisiteDAL preRequisiteDAL)
        {
            _preRequisiteDAL = preRequisiteDAL;
        }

        public async Task<bool> AddPreRequisiteAsync(PreRequisiteModel preRequisite)
        {
            return await _preRequisiteDAL.AddAsync(preRequisite);
        }

        public async Task<bool> DeletePreRequisiteAsync(int preRequisiteId)
        {
            return await _preRequisiteDAL.DeleteAsync(preRequisiteId);
        }

        public async Task<PreRequisiteModel> GetPreRequisiteByIdAsync(int preRequisiteId)
        {
            return await _preRequisiteDAL.GetByIdAsync(preRequisiteId);
        }

        public async Task<IEnumerable<PreRequisiteModel>> GetAllPreRequisitesAsync()
        {
            return await _preRequisiteDAL.GetAllAsync();
        }

        public async Task<bool> UpdatePreRequisiteAsync(PreRequisiteModel preRequisite)
        {
            return await _preRequisiteDAL.UpdateAsync(preRequisite);
        }
    }
}