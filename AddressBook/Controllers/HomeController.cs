using System.Linq;
using System.Threading;
using System.Web.Mvc;
using AddressBook.DataAccess;

namespace AddressBook.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    private readonly AddressBookContext _db = new AddressBookContext();


    [AllowAnonymous]
    public ActionResult ChangeCulture(string lang)
    {
      CultureHelper.ChangeLanguage(lang);
      return Redirect(Request.UrlReferrer.AbsolutePath);
    }

    //
    // GET: /Person/
    [AllowAnonymous]
    public ActionResult Index()
    {
      return View(_db.Persons.ToList());
    }

    //
    // GET: /Person/
    [AllowAnonymous]
    public ActionResult SearchByName(string personName)
    {
      IQueryable<Person> persons = _db.Persons;
      if(!string.IsNullOrWhiteSpace(personName))
        persons = persons.Where(p => (p.Name + " " + p.SurName).Contains(personName));

      Thread.Sleep(1000);
      return PartialView(persons.ToList());
    }

    //
    // GET: /Person/Details/5
    [AllowAnonymous]
    public ActionResult Details(int id = 0)
    {
      Person person = _db.Persons.Find(id);
      if (person == null)
      {
        return HttpNotFound();
      }
      return View(person);
    }

    //
    // GET: /Person/Create
    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Person/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Person person)
    {
      if (ModelState.IsValid)
      {
        _db.Persons.Add(person);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(person);
    }

    //
    // GET: /Person/Edit/5
    public ActionResult Edit(int id = 0)
    {
      Person person = _db.Persons.Find(id);
      if (person == null)
      {
        return HttpNotFound();
      }
      return View(person);
    }

    //
    // POST: /Person/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Person person)
    {
      if (ModelState.IsValid)
      {
        _db.Entry(person).State = System.Data.Entity.EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      return View(person);
    }

    //
    // GET: /Person/Delete/5
    public ActionResult Delete(int id = 0)
    {
      var person = _db.Persons.Find(id);
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
      var person = _db.Persons.Find(id);
      _db.Persons.Remove(person);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
      _db.Dispose();
      base.Dispose(disposing);
    }
  }
}