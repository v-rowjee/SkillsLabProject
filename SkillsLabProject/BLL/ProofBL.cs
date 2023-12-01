using SkillsLabProject.DAL;
using SkillsLabProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsLabProject.BLL
{
    public interface IProofBL
    {
        IEnumerable<ProofModel> GetAllProofs();
        ProofModel GetProofById(int proofId);
        bool AddProof(ProofModel model);
        bool UpdateProof(ProofModel model);
        bool DeleteProof(int proofId);

    }
    public class ProofBL : IProofBL
    {
        private readonly IProofDAL _proofDAL;

        public ProofBL(IProofDAL proofDAL)
        {
            _proofDAL = proofDAL;
        }

        public bool AddProof(ProofModel proof)
        {
            return _proofDAL.Add(proof);
        }
        public bool DeleteProof(int proofId)
        {
            return _proofDAL.Delete(proofId);
        }
        public ProofModel GetProofById(int proofId)
        {
            return _proofDAL.GetById(proofId);
        }
        public IEnumerable<ProofModel> GetAllProofs()
        {
            return _proofDAL.GetAll();
        }
        public bool UpdateProof(ProofModel proof)
        {
            return _proofDAL.Update(proof);
        }
    }
}