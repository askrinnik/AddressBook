using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBook.Core.DataAccess
{
  public class Person
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
}