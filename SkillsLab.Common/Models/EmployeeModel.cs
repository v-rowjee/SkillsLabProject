using SkillsLabProject.Common.Enums;

namespace SkillsLabProject.Common.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NIC { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DepartmentModel Department { get; set; }
        public Role Role { get; set; }
    }
}
