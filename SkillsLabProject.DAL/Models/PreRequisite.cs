using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.Models
{
    public class PreRequisite
    {
        [Key]
        public int PreRequisiteId { get; set; }
        public int TrainingId { get; set; }
        public string Detail { get; set; }
    }
}
