using ApiCatalogo.Context;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        public CategoriasController(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
            _configuration = configuration;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                return _uow.CategoriaRepository.GetCategoriasProdutos().ToList();
            } catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter as categorias do banco de dados");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                return _uow.CategoriaRepository.Get().ToList();
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter as categorias do banco de dados");
            }

        }

        [HttpGet("/saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("{id}", Name = "ObterCategoria" )]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var cat = _uow.CategoriaRepository.GetById(cat => cat.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                return cat;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter categoria do banco de dados");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            try
            {
                _uow.CategoriaRepository.Add(categoria);
                _uow.Commit();
                return CreatedAtRoute("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao inserir nova categoria no banco de dados");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                _uow.CategoriaRepository.Update(categoria);
                _uow.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao alterar categoria no banco de dados");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            try
            {
                var cat = _uow.CategoriaRepository.GetById(cat => cat.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                _uow.CategoriaRepository.Delete(cat);
                _uow.Commit();
                return cat;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar categoria do banco de dados");
            }
        }
    }
}
