using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace AddressBook.Ang
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
      config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());
    }
  }
}
