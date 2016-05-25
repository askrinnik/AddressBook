using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AddressBook.DataAccess;

namespace AddressBook.Controllers
{
  public class PhoneController : Controller
  {
    private AddressBookContext db = new AddressBookContext();

    //
    // GET: /Phone/

    public ActionResult Index(int id)
    {
      ViewBag.PersonId = id;
      var phones = db.Phones.Include(p => p.Person).Where(p => p.PersonId == id);
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
        db.Phones.Add(phone);
        db.SaveChanges();
        return RedirectToAction("Index", new {id = phone.PersonId});
      }

      return View(phone);
    }

    //
    // GET: /Phone/Edit/5

    public ActionResult Edit(int id)
    {
      var phone = db.Phones.Find(id);
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

      db.Entry(phone).State = System.Data.Entity.EntityState.Modified;
      db.SaveChanges();
      return RedirectToAction("Index", new { id = phone.PersonId });
    }

    //
    // GET: /Phone/Delete/5

    public ActionResult Delete(int id = 0)
    {
      var phone = db.Phones.Find(id);
      if (phone == null)
        return HttpNotFound();

      db.Phones.Remove(phone);
      db.SaveChanges();
      return RedirectToAction("Index", new { id = phone.PersonId });
    }


    protected override void Dispose(bool disposing)
    {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}