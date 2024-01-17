using System;
using System.Collections.Generic;

namespace SkillsLabProject.Common.Models.ViewModels
{
    public class TrainingViewModel
    {
        public int TrainingId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public int Capacity { get; set; }
        public DepartmentModel PriorityDepartment { get; set; }
        public List<string> PreRequisites { get; set;}
        public bool IsClosed {  get; set; }
        public int SeatsLeft {  get; set; }
        public List<EnrollmentModel> Enrollments { get; set; }
    }
}
