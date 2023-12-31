﻿using SkillsLabProject.Common.Enums;
using SkillsLabProject.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Models.ViewModels
{
    public class EnrollmentViewModel
    {
        public int EnrollmentId { get; set; }
        public EmployeeModel Employee { get; set; }
        public TrainingModel Training { get; set; }
        public List<ProofModel> Proofs { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
