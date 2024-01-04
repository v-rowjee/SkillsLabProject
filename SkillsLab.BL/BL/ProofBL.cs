using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsLabProject.BL.BL
{
    public interface IProofBL
    {
        Task<IEnumerable<ProofModel>> GetAllProofsAsync();
        Task<ProofModel> GetProofByIdAsync(int proofId);
        Task<bool> AddProofAsync(ProofModel model);
        Task<bool> UpdateProofAsync(ProofModel model);
        Task<bool> DeleteProofAsync(int proofId);
    }

    public class ProofBL : IProofBL
    {
        private readonly IProofDAL _proofDAL;

        public ProofBL(IProofDAL proofDAL)
        {
            _proofDAL = proofDAL;
        }

        public async Task<bool> AddProofAsync(ProofModel proof)
        {
            return await _proofDAL.AddAsync(proof);
        }

        public async Task<bool> DeleteProofAsync(int proofId)
        {
            return await _proofDAL.DeleteAsync(proofId);
        }

        public async Task<ProofModel> GetProofByIdAsync(int proofId)
        {
            return await _proofDAL.GetByIdAsync(proofId);
        }

        public async Task<IEnumerable<ProofModel>> GetAllProofsAsync()
        {
            return await _proofDAL.GetAllAsync();
        }

        public async Task<bool> UpdateProofAsync(ProofModel proof)
        {
            return await _proofDAL.UpdateAsync(proof);
        }
    }
}