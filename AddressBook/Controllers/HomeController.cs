using System.Linq;
using System.Threading;
using System.Web.Mvc;
using AddressBook.Core.DataAccess;

namespace AddressBook.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private readonly IAddressBookRepository _repository;// = new AddressBookRepository(new AddressBookContext());

    public HomeController(IAddressBookRepository repository)
    {
      _repository = repository;
    }

    [AllowAnonymous]
    public ActionResult ChangeCulture(string lang)
    {
      CultureHelper.ChangeLanguage(lang);
      return Redirect(Request.UrlReferrer?.AbsolutePath);
    }

    // GET: /Person/
    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(_repository.GetPersons());
    }

    // GET: /Person/
    [AllowAnonymous]
    public ActionResult SearchByName(string personName)
    {

      Thread.Sleep(1000);
      return PartialView(_repository.GetPersons(personName));
    }

    // GET: /Person/Details/5
    [AllowAnonymous]
    public ActionResult Details(int id = 0)
    {
      var person = _repository.GetPerson(id);
      if (person == null)
        return HttpNotFound();
      return View(person);
    }

    // GET: /Person/Create
    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
      return View();
    }

    // POST: /Person/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Person person)
    {
      if (ModelState.IsValid)
      {
        _repository.CreatePerson(person);
        return RedirectToAction("Index");
      }

      return View(person);
    }

    // GET: /Person/Edit/5
    public ActionResult Edit(int id = 0)
    {
      var person = _repository.GetPerson(id);
      if (person == null)
        return HttpNotFound();
      return View(person);
    }

    // POST: /Person/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Person person)
    {
      if (!ModelState.IsValid)
        return View(person);

      _repository.EditPerson(person);
      return RedirectToAction("Index");
    }

    // GET: /Person/Delete/5
    public ActionResult Delete(int id = 0)
    {
      var person = _repository.GetPerson(id);
      if (person == null)
        return HttpNotFound();
      
      return View(person);
    }

    //
    // POST: /Person/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      _repository.DeletePerson(id);
      return RedirectToAction("Index");
    }
  }
}