using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Models
{
    public class ProofModel
    {
        public int? ProofId { get; set; }
        public int EnrollmentId { get; set; }
        public string Attachment {  get; set; }
    }
}
