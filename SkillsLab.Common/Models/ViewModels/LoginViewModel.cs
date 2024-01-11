using SkillsLabProject.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Role CurrentRole { get; set; }
    }
}
