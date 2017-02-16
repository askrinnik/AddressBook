using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using AddressBook.Core.DataAccess;

namespace AddressBook.Ang.Controllers
{
  [RoutePrefix("api/Phone")]
  public class PhoneController : ApiController
  {
    private readonly IAddressBookRepository _repository; 

    public PhoneController(IAddressBookRepository repository)
    {
      _repository = repository;
    }

    // GET: api/Phone/Person/id
    [HttpGet]
    [Route("Person/{id}")]
    public async Task<IEnumerable<Phone>> GetAllPhonesForPerson(int id)
    {
      return await _repository.GetPersonPhones(id);
    }


    // POST: api/Phone
    [HttpPost]
    [Route("")]
    public Phone Post([FromBody] Phone phone)
    {
      // there is no $resource.$update() in angular
      // so, we use $save() and check primary key
      if (phone.Id > 0)
        _repository.EditPhone(phone);
      else
        _repository.CreatePhone(phone);
      return phone;
    }

    // PUT: api/Phone/5
    [HttpPut]
    [Route("{id}")]
    public void Put(int id, [FromBody] Phone phone)
    {
      // not used
      // there is no $resource.$update() in angular
      _repository.EditPhone(phone);
    }

    // DELETE: api/Phone/5
    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id)
    {
      _repository.DeletePhone(id);
    }

  }
}
