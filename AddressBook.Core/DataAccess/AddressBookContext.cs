using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AddressBook.Core.DataAccess
{
  public class AddressBookContext : DbContext
  {
    public AddressBookContext()
      : base("DefaultConnection")
    {
      Configuration.ProxyCreationEnabled = false;
    }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    } 

    public DbSet<Person> Persons { get; set; }
    public DbSet<Phone> Phones { get; set; }
  }
}