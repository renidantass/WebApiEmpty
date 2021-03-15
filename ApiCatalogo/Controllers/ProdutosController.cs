using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _context.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var prod = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
            if (prod == null)
            {
                return NotFound();
            }

            return prod;
        }

        [HttpPost]
        public  ActionResult Post([FromBody] Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Produto produto)
        {
            if(id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var prod = _context.Produtos.FirstOrDefault(prod => prod.ProdutoId == id);
            //var prod = _context.Produtos.Find(id);

            if (prod == null)
            {
                return BadRequest();
            }

            _context.Produtos.Remove(prod);
            _context.SaveChanges();
            return prod;
        }
    }
}
