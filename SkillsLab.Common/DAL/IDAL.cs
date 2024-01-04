using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.DAL
{
    public interface IDAL<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> AddAsync(T model);
        Task<bool> UpdateAsync(T model);
        Task<bool> DeleteAsync(int id);
    }
}
