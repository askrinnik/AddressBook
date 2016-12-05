using System.Collections.Generic;
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
    public IEnumerable<Person> Get()
    {
      return _repository.GetPersons();
    }

    // GET: api/Person/5
    public string Get(int id)
    {
      return "value";
    }

    // POST: api/Person
    public void Post([FromBody] string value)
    {
    }

    // PUT: api/Person/5
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE: api/Person/5
    public void Delete(int id)
    {
    }
  }
}
