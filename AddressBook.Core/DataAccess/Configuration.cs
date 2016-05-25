namespace AddressBook.Core.DataAccess
{
  using System;
  using System.Data.Entity.Migrations;
  using System.Linq;
  public sealed class AddressBookDbMigrationsConfiguration : DbMigrationsConfiguration<AddressBookContext>
  {
    public AddressBookDbMigrationsConfiguration()
    {
      AutomaticMigrationsEnabled = true;
    }

    protected override void Seed(AddressBookContext context)
    {
      if (context.Persons.Any())
        return;
      //  This method will be called after migrating to the latest version.
      var person = new Person
      {
        Name = "Иван",
        SurName = "Иванов",
        BirthDay = new DateTime(1970, 1, 1),
      };

      context.Persons.Add(person);

      context.SaveChanges();

      var phones = new[]
      {
        new Phone {Person = person, PhoneNumber = "11111"},
        new Phone {Person = person, PhoneNumber = "22222"},
      };
      foreach (var phone in phones)
        context.Phones.Add(phone);

      context.SaveChanges();

      //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
      //  to avoid creating duplicate seed data. E.g.
      //
      //    context.People.AddOrUpdate(
      //      p => p.FullName,
      //      new Person { FullName = "Andrew Peters" },
      //      new Person { FullName = "Brice Lambson" },
      //      new Person { FullName = "Rowan Miller" }
      //    );
      //
    }
  }
}
