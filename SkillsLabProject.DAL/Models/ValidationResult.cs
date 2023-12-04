using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.DAL.Models
{
    public class ValidationResult
    {
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}
