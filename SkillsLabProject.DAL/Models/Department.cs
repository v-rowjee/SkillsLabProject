using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Title { get; set; }
    }
}
