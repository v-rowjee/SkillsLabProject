using SkillsLabProject.Common.Enums;
using System;

namespace SkillsLabProject.Common.Models
{
    public class EnrollmentModel
    {
        public int EnrollmentId { get; set; }
        public int EmployeeId { get; set; }
        public int TrainingId { get; set; }
        public Status Status { get; set; }
        public string DeclinedReason { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
