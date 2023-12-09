using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Models
{
    public class AppUserModel
    {
        public int AppUserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int EmployeeId { get; set; }
    }
}
