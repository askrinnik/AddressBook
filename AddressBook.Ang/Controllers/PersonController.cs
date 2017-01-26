using System.Collections.Generic;
using System.Threading;
using System.Web.Http;
using AddressBook.Core.DataAccess;

namespace AddressBook.Ang.Controllers
{
  public class PersonController : ApiController
  {
    private readonly IAddressBookRepository _repository; // = new AddressBookRepository(new AddressBookContext());

    public PersonController(IAddressBookRepository repository)
    {
      _repository = repository;
    }

    // GET: api/Person
    [HttpGet]
    [Route("api/Person")]
    public IEnumerable<Person> GetAll()
    {
      return _repository.GetPersons();
    }

    // GET: api/Person/GetByName/:personName
    [HttpGet]
    [Route("api/Person/GetByName/{personName}")]
    public IEnumerable<Person> GetByName(string personName)
    {
      Thread.Sleep(1000);
      return _repository.GetPersons(personName);
    }

    // GET: api/Person/5
    [HttpGet]
    [Route("api/Person/{id}")]
    public Person Get(int id)
    {
      return _repository.GetPerson(id);
    }

    // POST: api/Person
    [HttpPost]
    [Route("api/Person")]
    public void Post([FromBody] Person person)
    {
      // $resource.$update() doesn't work  in angular
      // so, we use $save() and check primary key
      if (person.Id > 0)
        _repository.EditPerson(person);
      else
        _repository.CreatePerson(person);
    }

    // PUT: api/Person/5
    [HttpPut]
    [Route("api/Person/{id}")]
    public void Put(int id, [FromBody] Person person)
    {
      // not used
      // $resource.$update() doesn't work  in angular
      _repository.EditPerson(person);
    }

    // DELETE: api/Person/5
    [HttpDelete]
    [Route("api/Person/{id}")]
    public void Delete(int id)
    {
      _repository.DeletePerson(id);
    }

    [HttpGet]
    [Route("api/Person/getPhoneList")]
    public IEnumerable<PhoneListModel> GetPhoneList()
    {
      return _repository.GetPhoneList();
    }

    [HttpGet]
    [Route("api/Person/getPhoneCount")]
    public IEnumerable<PhoneCountModel> GetPhoneCount()
    {
      return _repository.GetPhoneCount();
    }

  }
}
