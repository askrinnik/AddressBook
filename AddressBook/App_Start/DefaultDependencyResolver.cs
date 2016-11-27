using AddressBook.Core.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook
{
  public class DefaultDependencyResolver : DependencyResolverBase
  {
    protected override void ConfigureServices(IServiceCollection services)
    {
      services.AddTransient<AddressBookContext>();
      services.AddTransient<IAddressBookRepository, AddressBookRepository>();
      AddControllersAsServices(services);
    }
  }

}