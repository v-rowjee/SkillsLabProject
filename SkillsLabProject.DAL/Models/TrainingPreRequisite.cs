using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.Models
{
    public class TrainingPreRequisite
    {
        [Key]
        public int TrainingId { get; set; }
        [Key]
        public int PreRequisiteId { get; set; }
    }
}
