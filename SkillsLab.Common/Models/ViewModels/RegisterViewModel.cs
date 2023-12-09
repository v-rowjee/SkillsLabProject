using SkillsLabProject.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string NIC { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
    }
}
