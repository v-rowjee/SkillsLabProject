using SkillsLabProject.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Models
{
    public class EnrollmentModel
    {
        public int EnrollmentId { get; set; }
        public int EmployeeId { get; set; }
        public int TrainingId { get; set; }
        public Status Status { get; set; }
        public string DeclinedReason { get; set; }
    }
}
