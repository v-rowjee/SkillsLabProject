using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.Models
{
    public class ProofModel
    {
        [Key]
        public int ProofId { get; set; }
        public int EnrollmentId { get; set; }
        public string Attachment {  get; set; }
    }
}
