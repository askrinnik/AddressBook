using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using AddressBook.Core.DataAccess;

namespace AddressBook.Ang
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      GlobalConfiguration.Configure(WebApiConfig.Register);

      Database.SetInitializer(new AddressBookDbIfNotExistsInitializer());
      GlobalConfiguration.Configuration.DependencyResolver = new DefaultDependencyResolver(); // for ApiControllers
      DependencyResolver.SetResolver(new DefaultDependencyResolver()); // For MVC controllers
    }
  }
}
