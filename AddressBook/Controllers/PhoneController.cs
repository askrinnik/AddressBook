using System.Threading.Tasks;
using System.Web.Mvc;
using AddressBook.Core.DataAccess;

namespace AddressBook.Controllers
{
  public class PhoneController : Controller
  {
    private readonly IAddressBookRepository _repository;// = new AddressBookRepository(new AddressBookContext());

    public PhoneController(IAddressBookRepository repository)
    {
      _repository = repository;
    }

    //
    // GET: /Phone/
    public async Task<ActionResult> Index(int id)
    {
      ViewBag.PersonId = id;
      var phones = await _repository.GetPersonPhones(id);
      return View(phones);
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
        _repository.CreatePhone(phone);
        return RedirectToAction("Index", new {id = phone.PersonId});
      }

      return View(phone);
    }

    //
    // GET: /Phone/Edit/5
    public ActionResult Edit(int id)
    {
      var phone = _repository.GetPhone(id);
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

      _repository.EditPhone(phone);
      return RedirectToAction("Index", new { id = phone.PersonId });
    }

    //
    // GET: /Phone/Delete/5

    public ActionResult Delete(int id = 0)
    {
      var phone = _repository.GetPhone(id);
      if (phone == null)
        return HttpNotFound();

      _repository.DeletePhone(phone);

      return RedirectToAction("Index", new { id = phone.PersonId });
    }

  }
}