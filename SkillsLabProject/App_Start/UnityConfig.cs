using SkillsLabProject.DAL;
using SkillsLabProject.BLL;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace SkillsLabProject
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

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

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}