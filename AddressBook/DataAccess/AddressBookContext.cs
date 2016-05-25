using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Mvc;

namespace AddressBook.DataAccess
{
  public class AddressBookContext : DbContext
  {
    public AddressBookContext()
      : base("DefaultConnection")
    {
    }
    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    } 

    public DbSet<Person> Persons { get; set; }
    public DbSet<Phone> Phones { get; set; }
  }

  public class Person
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [HiddenInput]
    public int Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Person_Name_Required")]
    [Display(ResourceType = typeof(Resources), Name = "Person_Name")]
    public string Name { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Person_SurName_Required")]
    [Display(ResourceType = typeof(Resources), Name = "Person_SurName")]
    public string SurName { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Person_BirthDay_Required")]
    [Display(ResourceType = typeof(Resources), Name = "Person_BirthDay")]
    [DataType(DataType.Date)]
//    [DisplayFormat(DataFormatString = "{0:d}")]
    [UIHint("DatePicker")]
    public DateTime BirthDay { get; set; }

    public IEnumerable<Phone> Phones { get; set; }

  }
  public class Phone
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Phone_PhoneNumber_Required")]
    [Display(ResourceType = typeof(Resources), Name = "Phone_PhoneNumber")]
    public string PhoneNumber { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }
  }

}