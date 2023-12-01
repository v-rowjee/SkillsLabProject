using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Models
{
    public class TrainingModel
    {
        public int TrainingId { get; set; }
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public int Capacity { get; set; }
        public int? PriorityDepartmentId { get; set; }

    }
}
