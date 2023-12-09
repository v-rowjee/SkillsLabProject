using SkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.DAL
{
    public interface IDAL<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Add(T model);
        bool Update(T model);
        bool Delete(int id);
    }
}
