using SkillsLabProject.DAL.DAL;
using SkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsLabProject.BL.BL
{
    public interface IDeclinedEnrollmentBL
    {
        IEnumerable<DeclinedEnrollmentModel> GetAllDeclinedEnrollments();
        DeclinedEnrollmentModel GetDeclinedEnrollmentById(int declinedEnrollmentId);
        bool AddDeclinedEnrollment(DeclinedEnrollmentModel model);
        bool UpdateDeclinedEnrollment(DeclinedEnrollmentModel model);
        bool DeleteDeclinedEnrollment(int declinedEnrollmentId);

    }
    public class DeclinedEnrollmentBL : IDeclinedEnrollmentBL
    {
        private readonly IDeclinedEnrollmentDAL _declinedEnrollmentDAL;

        public DeclinedEnrollmentBL(IDeclinedEnrollmentDAL declinedEnrollmentDAL)
        {
            _declinedEnrollmentDAL = declinedEnrollmentDAL;
        }

        public bool AddDeclinedEnrollment(DeclinedEnrollmentModel declinedEnrollment)
        {
            return _declinedEnrollmentDAL.Add(declinedEnrollment);
        }
        public bool DeleteDeclinedEnrollment(int declinedEnrollmentId)
        {
            return _declinedEnrollmentDAL.Delete(declinedEnrollmentId);
        }
        public DeclinedEnrollmentModel GetDeclinedEnrollmentById(int declinedEnrollmentId)
        {
            return _declinedEnrollmentDAL.GetById(declinedEnrollmentId);
        }
        public IEnumerable<DeclinedEnrollmentModel> GetAllDeclinedEnrollments()
        {
            return _declinedEnrollmentDAL.GetAll();
        }
        public bool UpdateDeclinedEnrollment(DeclinedEnrollmentModel declinedEnrollment)
        {
            return _declinedEnrollmentDAL.Update(declinedEnrollment);
        }
    }
}