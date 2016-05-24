using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AddressBook.DataAccess
{
  public class UsersContext : DbContext
  {
    public UsersContext()
      : base("DefaultConnection")
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
  }

  [Table("UserProfile")]
  public class UserProfile
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
  }

}