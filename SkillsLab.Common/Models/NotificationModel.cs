using SkillsLabProject.Common.Enums;
using System;

namespace SkillsLab.Common.Models
{
    public class NotificationModel
    {
        public int NotificationId { get; set; }
        public int EmployeeId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime ReceivedOn { get; set; }
        public Role EmployeeRole { get; set; }
    }
}
