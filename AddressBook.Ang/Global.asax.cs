using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
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

      // print routes description. It helps to understand, which controller method is invoked
      var routes = GetRoutesDescriptions(GlobalConfiguration.Configuration.Services.GetApiExplorer());
    }

    private static string GetRoutesDescriptions(IApiExplorer apiExplorer)
    {
      var sb = new StringBuilder();
      foreach (var api in apiExplorer.ApiDescriptions)
      {
        var descriptor = (System.Web.Http.Controllers.ReflectedHttpActionDescriptor) api.ActionDescriptor;
        var methodInfo = descriptor.MethodInfo;
        var methodParameters = string.Join(", ", methodInfo.GetParameters().Select(p => p.Name));
        sb.AppendLine($"{api.HttpMethod}: {api.RelativePath},   {string.Join(", ", api.ParameterDescriptions.Select(p => $"[{p.Source}]{p.Name}"))}");
        sb.AppendLine($"{methodInfo.ReturnType.Name} {descriptor.ControllerDescriptor.ControllerType.Name}.{methodInfo.Name}({methodParameters})");
        sb.AppendLine();
      }
      return sb.ToString();
    }
  }
}
