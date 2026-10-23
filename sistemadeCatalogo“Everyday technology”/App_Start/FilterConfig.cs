using System.Web;
using System.Web.Mvc;

namespace sistemadeCatalogo_Everyday_technology_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
