using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using edgerest.Models;
using edgerest.Repositories;

namespace edgerest.Controllers
{
    public class DataController : Controller
    {
        private readonly IPersonRepository _personRepository;

        public DataController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet("data/{id}")]
        public Person GetPerson(int id)
        {
            return _personRepository.GetById(id);
        }

        
        [HttpGet("data")]
        public List<string> GetData()
        {
            List<string> foo = new List<string>();
            foo.Add("abc");
            foo.Add("foo");
            foo.Add("foovsfds");
            return foo;
        }

        
    }
}
