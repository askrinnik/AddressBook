﻿using System.Collections.Generic;
using System.Threading;
using System.Web.Http;
using AddressBook.Core.DataAccess;

namespace AddressBook.Ang.Controllers
{
  [RoutePrefix("api/Person")]
  public class PersonController : ApiController
  {
    private readonly IAddressBookRepository _repository; // = new AddressBookRepository(new AddressBookContext());

    public PersonController(IAddressBookRepository repository)
    {
      _repository = repository;
    }

    // GET: api/Person
    [HttpGet]
    [Route("")]
    public IEnumerable<Person> GetAll()
    {
      return _repository.GetPersons();
    }

    // GET: api/Person/GetByName/:personName
    [HttpGet]
    [Route("GetByName/{personName}")]
    public IEnumerable<Person> GetByName(string personName)
    {
      Thread.Sleep(1000);
      return _repository.GetPersons(personName);
    }

    // GET: api/Person/5
    [HttpGet]
    [Route("{id}")]
    public Person Get(int id)
    {
      return _repository.GetPerson(id);
    }

    // POST: api/Person
    [HttpPost]
    [Route("")]
    public void Post([FromBody] Person person)
    {
      // there is no $resource.$update() in angular
      // so, we use $save() and check primary key
      if (person.Id > 0)
        _repository.EditPerson(person);
      else
        _repository.CreatePerson(person);
    }

    // PUT: api/Person/5
    [HttpPut]
    [Route("{id}")]
    public void Put(int id, [FromBody] Person person)
    {
      // not used
      // there is no $resource.$update() in angular
      _repository.EditPerson(person);
    }

    // DELETE: api/Person/5
    [HttpDelete]
    [Route("{id}")]
    public void Delete(int id)
    {
      _repository.DeletePerson(id);
    }

    [HttpGet]
    [Route("getPhoneListReport")]
    public IEnumerable<PhoneListReportModel> GetPhoneListReport()
    {
      return _repository.GetPhoneListReport();
    }

    [HttpGet]
    [Route("getPhoneCountReport")]
    public IEnumerable<PhoneCountReportModel> GetPhoneCountReport()
    {
      return _repository.GetPhoneCountReport();
    }

  }
}
