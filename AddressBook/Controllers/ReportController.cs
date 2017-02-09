using System.Linq;
using System.Web.Mvc;
using AddressBook.Core.DataAccess;

namespace AddressBook.Controllers
{
  public class ReportController : Controller
  {
    private readonly IAddressBookRepository _repository; 

    public ReportController(IAddressBookRepository repository)
    {
      _repository = repository;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult PhoneList()
    {
      var list = _repository.GetPhoneListReport();

      return View(list.ToArray());
    }


    public ActionResult PhoneCount()
    {
      var phoneCounts = _repository.GetPhoneCountReport();
      return View(phoneCounts.ToArray());
    }

  }
}
