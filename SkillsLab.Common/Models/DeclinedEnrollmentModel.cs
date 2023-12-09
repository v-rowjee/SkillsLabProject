using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Models
{
    public class DeclinedEnrollmentModel
    {
        public int? DeclinedEnrollmentId { get; set; }
        public int EnrollmentId { get; set;}
        public string Reason { get; set;}
    }
}
