using SkillsLabProject.BL.BL;
using SkillsLabProject.BL.Services;
using SkillsLabProject.DAL.DAL;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace SkillsLabProject
{
    public static class UnityConfig
    {
        public static IUnityContainer Container { get; internal set; }
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IAppUserBL, AppUserBL>();
            container.RegisterType<IAppUserDAL, AppUserDAL>();

            container.RegisterType<IEmployeeBL, EmployeeBL>();
            container.RegisterType<IEmployeeDAL, EmployeeDAL>();

            container.RegisterType<IEnrollmentBL, EnrollmentBL>();
            container.RegisterType<IEnrollmentDAL, EnrollmentDAL>();

            container.RegisterType<ITrainingBL, TrainingBL>();
            container.RegisterType<ITrainingDAL,  TrainingDAL>();

            container.RegisterType<IPreRequisiteBL, PreRequisiteBL>();
            container.RegisterType<IPreRequisiteDAL, PreRequisiteDAL>();

            container.RegisterType<IProofBL, ProofBL>();
            container.RegisterType<IProofDAL, ProofDAL>();

            container.RegisterType<IDeclinedEnrollmentBL, DeclinedEnrollmentBL>();
            container.RegisterType<IDeclinedEnrollmentDAL, DeclinedEnrollmentDAL>();

            container.RegisterType<IDepartmentBL, DepartmentBL>();
            container.RegisterType<IDepartmentDAL, DepartmentDAL>();

            container.RegisterType<IEmailService, EmailService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            Container = container;
        }
    }
}