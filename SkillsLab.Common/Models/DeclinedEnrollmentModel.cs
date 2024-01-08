namespace SkillsLabProject.Common.Models
{
    public class DeclinedEnrollmentModel
    {
        public int? DeclinedEnrollmentId { get; set; }
        public int EnrollmentId { get; set;}
        public string Reason { get; set;}
    }
}
