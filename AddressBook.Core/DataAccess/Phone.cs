using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AddressBook.Core.DataAccess
{
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