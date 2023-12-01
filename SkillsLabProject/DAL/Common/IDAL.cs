using SkillsLabProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.Common
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
