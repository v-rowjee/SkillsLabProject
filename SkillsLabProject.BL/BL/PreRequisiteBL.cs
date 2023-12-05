using SkillsLabProject.BLL;
using SkillsLabProject.DAL.Models;
using SkillsLabProject.DAL.RepositoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.BL.BL
{
    public interface IPreRequisiteBL { }
    public class PreRequisiteBL : IPreRequisiteBL
    {
        private readonly IRepositoryDAL<PreRequisite> _preRequisiteDAL;

        public PreRequisiteBL(IRepositoryDAL<PreRequisite> preRequisiteDAL)
        {
            _preRequisiteDAL = preRequisiteDAL;
        }
    }
}
