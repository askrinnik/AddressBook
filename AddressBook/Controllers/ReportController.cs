using System.Linq;
using System.Web.Mvc;
using AddressBook.Core.DataAccess;
using AddressBook.Models;

namespace AddressBook.Controllers
{
  public class ReportController : Controller
  {
    private readonly AddressBookContext _db = new AddressBookContext();

    protected override void Dispose(bool disposing)
    {
      _db.Dispose();
      base.Dispose(disposing);
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult PhoneList()
    {
      var list = from person in _db.Persons
                 join phone in _db.Phones on person.Id equals phone.PersonId
                 select
                   new PhoneList
                   {
                     PersonId = person.Id,
                     PersonName = person.Name + " " + person.SurName,
                     PhoneId = phone.Id,
                     PersonPhone = phone.PhoneNumber
                   };

      return View(list.ToArray());
    }

    public ActionResult PhoneCount()
    {
      var phoneCounts =
        from person in _db.Persons
        select
          new PhoneCount
          {
            PersonId = person.Id,
            PersonName = person.Name + " " + person.SurName,
            Phones = _db.Phones.Count(p => p.PersonId == person.Id)
          };
      return View(phoneCounts.ToArray());
    }
  }
}
