using System.Web.Http.Dependencies;
using AddressBook.Core.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook.Ang
{
  public class DefaultDependencyResolver : DependencyResolverBase, IDependencyResolver
  {
    protected override void ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<AddressBookContext>();
      services.AddTransient<IAddressBookRepository, AddressBookRepository>();
      AddControllersAsServices(services);
    }

    public void Dispose()
    {
      //throw new System.NotImplementedException();
    }

    public IDependencyScope BeginScope()
    {
      return new DefaultDependencyResolver();
    }
  }

}