using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AddressBook.Core.DataAccess;

namespace AddressBook.Controllers
{
  public class PhoneController : Controller
  {
    private readonly AddressBookContext _db = new AddressBookContext();

    //
    // GET: /Phone/
    public ActionResult Index(int id)
    {
      ViewBag.PersonId = id;
      var phones = _db.Phones.Include(p => p.Person).Where(p => p.PersonId == id);
      return View(phones.ToList());
    }

    //
    // GET: /Phone/Create
    public ActionResult Create(int id)
    {
      var phone = new Phone { PersonId = id };
      return View(phone);
    }

    //
    // POST: /Phone/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Phone phone)
    {
      if (ModelState.IsValid)
      {
        _db.Phones.Add(phone);
        _db.SaveChanges();
        return RedirectToAction("Index", new {id = phone.PersonId});
      }

      return View(phone);
    }

    //
    // GET: /Phone/Edit/5
    public ActionResult Edit(int id)
    {
      var phone = _db.Phones.Find(id);
      if (phone == null)
        return HttpNotFound();
      
      return View(phone);
    }

    //
    // POST: /Phone/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Phone phone)
    {
      if (!ModelState.IsValid) return 
        View(phone);

      _db.Entry(phone).State = EntityState.Modified;

      _db.SaveChanges();
      return RedirectToAction("Index", new { id = phone.PersonId });
    }

    //
    // GET: /Phone/Delete/5

    public ActionResult Delete(int id = 0)
    {
      var phone = _db.Phones.Find(id);
      if (phone == null)
        return HttpNotFound();

      _db.Phones.Remove(phone);
      _db.SaveChanges();
      return RedirectToAction("Index", new { id = phone.PersonId });
    }


    protected override void Dispose(bool disposing)
    {
      _db.Dispose();
      base.Dispose(disposing);
    }
  }
}