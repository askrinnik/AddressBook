using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook
{
  public abstract class DependencyResolverBase : IDependencyResolver
  {
    private readonly IServiceProvider _serviceProvider;

    protected DependencyResolverBase()
    {
      _serviceProvider = GetServiceProvider();
    }

    public object GetService(Type serviceType)
    {
      return _serviceProvider.GetService(serviceType);
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
      return _serviceProvider.GetServices(serviceType);
    }

    protected static void AddControllersAsServices(IServiceCollection services)
    {
      var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
        .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
        .Where(
          t =>
            typeof(IController).IsAssignableFrom(t) ||
            t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
      foreach (var controllerType in controllerTypes)
        services.AddTransient(controllerType);
    }
    private IServiceProvider GetServiceProvider()
    {
      var services = new ServiceCollection();
      ConfigureServices(services);

      return services.BuildServiceProvider();
    }

    protected abstract void ConfigureServices(IServiceCollection services);
  }
}