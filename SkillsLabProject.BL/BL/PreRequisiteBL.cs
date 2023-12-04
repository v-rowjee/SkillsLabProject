using SkillsLabProject.DAL;
using SkillsLabProject.DAL.Common;
using SkillsLabProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsLabProject.BLL
{
    public interface IPreRequisiteBL
    {
        IEnumerable<PreRequisiteModel> GetAllPreRequisites();
        PreRequisiteModel GetPreRequisiteById(int preRequisiteId);
        bool AddPreRequisite(PreRequisiteModel model);
        bool UpdatePreRequisite(PreRequisiteModel model);
        bool DeletePreRequisite(int preRequisiteId);
    }
    public class PreRequisiteBL : IPreRequisiteBL
    {
        private readonly IPreRequisiteDAL _preRequisiteDAL;

        public PreRequisiteBL(IPreRequisiteDAL preRequisiteDAL)
        {
            _preRequisiteDAL = preRequisiteDAL;
        }

        public bool AddPreRequisite(PreRequisiteModel preRequisite)
        {
            return _preRequisiteDAL.Add(preRequisite);
        }
        public bool DeletePreRequisite(int preRequisiteId)
        {
            return _preRequisiteDAL.Delete(preRequisiteId);
        }
        public PreRequisiteModel GetPreRequisiteById(int preRequisiteId)
        {
            return _preRequisiteDAL.GetById(preRequisiteId);
        }
        public IEnumerable<PreRequisiteModel> GetAllPreRequisites()
        {
            return _preRequisiteDAL.GetAll();
        }
        public bool UpdatePreRequisite(PreRequisiteModel preRequisite)
        {
            return _preRequisiteDAL.Update(preRequisite);
        }
    }
}