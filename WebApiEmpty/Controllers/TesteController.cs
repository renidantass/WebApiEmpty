using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiEmpty.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "teste", "teste2" };
        }

        [HttpGet("pessoas")]
        public ActionResult<IEnumerable<Pessoa>> GetPessoas()
        {
            return new[]
            {
                new Pessoa { Nome = "Reni" },
                new Pessoa { Nome = "João" },
                new Pessoa { Nome = "Macoratti" }
            };
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }

    public class Pessoa
    {
        public string Nome { get; set; }
    }
}
