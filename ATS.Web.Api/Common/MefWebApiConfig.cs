using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Web.Http;
using Library.Core.Bootstrapper;

namespace ATS.Web.Api.Common
{
    public class MefWebApiConfig
    {
        public static void Register()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            CompositionContainer container = MefLoader.Init(catalog.Catalogs);

           // DependencyResolver.SetResolver(new MefDependencyResolver(container)); // MVC controllers
            GlobalConfiguration.Configuration.DependencyResolver = new MefApiDependencyResolver(container); // Web API controllers
        }
    }
}