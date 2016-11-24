using System.Data.Entity;

namespace AddressBook.Core.DataAccess
{
  using System.Data.Entity.Migrations;

  public sealed class AddressBookDbMigrationsConfiguration : DbMigrationsConfiguration<AddressBookContext>
  {
    public AddressBookDbMigrationsConfiguration()
    {
      AutomaticMigrationsEnabled = true;
    }

    protected override void Seed(AddressBookContext context)
    {
      context.InitializeEmptyDatabase();
    }

  }

  public sealed class AddressBookDbIfNotExistsInitializer : DropCreateDatabaseIfModelChanges<AddressBookContext>
  {
    protected override void Seed(AddressBookContext context)
    {
      context.InitializeEmptyDatabase();
    }
  }
}
