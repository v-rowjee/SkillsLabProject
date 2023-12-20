using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Models.ViewModels
{
    public class EmailViewModel
    {
        public EmployeeModel Employee { get; set; }
        public EmployeeModel Manager { get; set; }
        public TrainingModel Training { get; set; }
    }
}
