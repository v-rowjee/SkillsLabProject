using SkillsLabProject.Common.Log;
using System.Web;
using System.Web.Mvc;

namespace SkillsLabProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
