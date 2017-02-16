using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Core.DataAccess
{
  public class AddressBookRepository : IAddressBookRepository
  {
    private readonly AddressBookContext _db;

    public AddressBookRepository(AddressBookContext context)
    {
      _db = context;
    }

    public Person[] GetPersons()
    {
      return _db.Persons.AsNoTracking().ToArray();
    }
    public Person[] GetPersons(string personName)
    {
      IQueryable<Person> persons = _db.Persons;
      if (!string.IsNullOrWhiteSpace(personName))
        persons = persons.Where(p => (p.Name + " " + p.SurName).Contains(personName));
      return persons.AsNoTracking().ToArray();
    }

    public Person GetPerson(int id)
    {
      return _db.Persons.Find(id);
    }

    public int CreatePerson(Person person)
    {
      _db.Persons.Add(person);
      _db.SaveChanges();
      return person.Id;
    }
    public void EditPerson(Person person)
    {
      _db.Entry(person).State = EntityState.Modified;
      _db.SaveChanges();
    }
    public void DeletePerson(int id)
    {
      var person = _db.Persons.Find(id);
      DeletePerson(person);
    }
    public void DeletePerson(Person person)
    {
      _db.Persons.Remove(person);
      _db.SaveChanges();
    }

    public async Task<IEnumerable<Phone>>  GetPersonPhones(int personId)
    {
      return await _db.Phones.Include(p => p.Person).Where(p => p.PersonId == personId).AsNoTracking().ToArrayAsync();
    }

    public void CreatePhone(Phone phone)
    {
      _db.Phones.Add(phone);
      _db.SaveChanges();
    }

    public Phone GetPhone(int id)
    {
      return _db.Phones.Find(id);
    }

    public void EditPhone(Phone phone)
    {
      _db.Entry(phone).State = EntityState.Modified;
      _db.SaveChanges();
    }

    public void DeletePhone(Phone phone)
    {
      _db.Phones.Remove(phone);
      _db.SaveChanges();
    }
    public void DeletePhone(int id)
    {
      var phone = _db.Phones.Find(id);
      DeletePhone(phone);
    }

    public IQueryable<PhoneListReportModel> GetPhoneListReport()
    {
      var list = from person in _db.Persons
                 join phone in _db.Phones on person.Id equals phone.PersonId
                 select
                 new PhoneListReportModel
                 {
                   PersonId = person.Id,
                   PersonName = person.Name + " " + person.SurName,
                   PhoneId = phone.Id,
                   PersonPhone = phone.PhoneNumber
                 };
      return list;
    }

    public IQueryable<PhoneCountReportModel> GetPhoneCountReport()
    {
      var phoneCounts =
        from person in _db.Persons
        select
        new PhoneCountReportModel
        {
          PersonId = person.Id,
          PersonName = person.Name + " " + person.SurName,
          Phones = _db.Phones.Count(p => p.PersonId == person.Id)
        };
      return phoneCounts;
    }

  }
}
