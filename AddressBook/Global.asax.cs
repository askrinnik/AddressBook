using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AddressBook.Core.DataAccess;

namespace AddressBook
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();

      WebApiConfig.Register(GlobalConfiguration.Configuration);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      AuthConfig.InitalizeWebSecurity();
      AuthConfig.RegisterAuth();

      Database.SetInitializer(new AddressBookDbIfNotExistsInitializer());
      //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AddressBookContext, AddressBookDbMigrationsConfiguration>());

      GlobalConfiguration.Configuration.DependencyResolver = new DefaultDependencyResolver(); // for ApiControllers
      DependencyResolver.SetResolver(new DefaultDependencyResolver()); // For MVC controllers
    }
    protected void Application_BeginRequest()
    {
      CultureHelper.ChangeCulture();
    }
  }
}