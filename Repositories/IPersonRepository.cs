using System.Collections.Generic;
using edgerest.Models;

namespace edgerest.Repositories
{
    public interface IPersonRepository
    {
        Person GetById(int id);
        List<Person> GetAll();
        int GetCount();
        void Remove();
        string Save(Person person);
    }
}