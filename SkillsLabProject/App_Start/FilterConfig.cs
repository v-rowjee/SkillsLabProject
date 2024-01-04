using SkillsLab.Common.Exceptions;
using System.Web.Mvc;

namespace SkillsLabProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalExceptionHandler());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
