using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using SkillsLabProject.Common.DAL;
using SkillsLabProject.DAL.RepositoryDAL;
using SkillsLabProject.BL.RepositoryBL;
using SkillsLabProject.BLL;

namespace SkillsLabProject
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IDataAccessLayer, DataAccessLayer>();
            container.RegisterType<IAppUserBL, AppUserBL>();
            container.RegisterType(typeof(IRepositoryDAL<>), typeof(RepositoryDAL<>));
            container.RegisterType(typeof(IRepositoryBL<>), typeof(RepositoryBL<>));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}