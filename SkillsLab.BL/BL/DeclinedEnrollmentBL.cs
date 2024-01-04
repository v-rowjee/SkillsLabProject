using System.Collections.Generic;
using System.Threading.Tasks;
using SkillsLabProject.Common.Models;
using SkillsLabProject.DAL.DAL;

namespace SkillsLabProject.BL.BL
{
    public interface IDeclinedEnrollmentBL
    {
        Task<IEnumerable<DeclinedEnrollmentModel>> GetAllDeclinedEnrollmentsAsync();
        Task<DeclinedEnrollmentModel> GetDeclinedEnrollmentByIdAsync(int declinedEnrollmentId);
        Task<bool> AddDeclinedEnrollmentAsync(DeclinedEnrollmentModel model);
        Task<bool> UpdateDeclinedEnrollmentAsync(DeclinedEnrollmentModel model);
        Task<bool> DeleteDeclinedEnrollmentAsync(int declinedEnrollmentId);
    }

    public class DeclinedEnrollmentBL : IDeclinedEnrollmentBL
    {
        private readonly IDeclinedEnrollmentDAL _declinedEnrollmentDAL;

        public DeclinedEnrollmentBL(IDeclinedEnrollmentDAL declinedEnrollmentDAL)
        {
            _declinedEnrollmentDAL = declinedEnrollmentDAL;
        }

        public async Task<bool> AddDeclinedEnrollmentAsync(DeclinedEnrollmentModel declinedEnrollment)
        {
            return await _declinedEnrollmentDAL.AddAsync(declinedEnrollment);
        }

        public async Task<bool> DeleteDeclinedEnrollmentAsync(int declinedEnrollmentId)
        {
            return await _declinedEnrollmentDAL.DeleteAsync(declinedEnrollmentId);
        }

        public async Task<DeclinedEnrollmentModel> GetDeclinedEnrollmentByIdAsync(int declinedEnrollmentId)
        {
            return await _declinedEnrollmentDAL.GetByIdAsync(declinedEnrollmentId);
        }

        public async Task<IEnumerable<DeclinedEnrollmentModel>> GetAllDeclinedEnrollmentsAsync()
        {
            return await _declinedEnrollmentDAL.GetAllAsync();
        }

        public async Task<bool> UpdateDeclinedEnrollmentAsync(DeclinedEnrollmentModel declinedEnrollment)
        {
            return await _declinedEnrollmentDAL.UpdateAsync(declinedEnrollment);
        }
    }
}
