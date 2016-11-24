using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

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

    public void InitializeEmptyDatabase()
    {
      var person1 = new Person
      {
        Name = "Иван",
        SurName = "Иванов",
        BirthDay = new DateTime(1970, 1, 1),
      };
      Persons.Add(person1);

      var person2 = new Person
      {
        Name = "Петр",
        SurName = "Петров",
        BirthDay = new DateTime(1972, 2, 2),
      };
      Persons.AddRange(new []{person1, person2});

      // approach #1: add phones to person object
      var phones = new[]
      {
        new Phone {/*Person = person1,*/ PhoneNumber = "11111"},
        new Phone {/*Person = person1,*/ PhoneNumber = "22222"},
      };
      person1.Phones.AddRange(phones);

      //approach #2: add phones to Phones table. Reference to person 
      Phones.Add(new Phone {Person = person2, PhoneNumber = "3333"});


      SaveChanges();

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