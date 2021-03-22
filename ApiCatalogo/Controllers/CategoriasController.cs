using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            try
            {
                var categorias = _uow.CategoriaRepository.GetCategoriasProdutos().ToList();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDTO;
            } catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter as categorias do banco de dados");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            try
            {
                var categorias = _uow.CategoriaRepository.Get().ToList();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDTO;
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
        public ActionResult<CategoriaDTO> Get(int id)
        {
            try
            {
                var cat = _uow.CategoriaRepository.GetById(cat => cat.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                var catDTO = _mapper.Map<CategoriaDTO>(cat);

                return catDTO;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter categoria do banco de dados");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _uow.CategoriaRepository.Add(categoria);
                _uow.Commit();
                return CreatedAtRoute("ObterCategoria", new { id = categoriaDto.CategoriaId }, categoriaDto);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao inserir nova categoria no banco de dados");
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uow.CategoriaRepository.Update(categoria);
                _uow.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao alterar categoria no banco de dados");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<CategoriaDTO> Delete(int id)
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

                var catDto = _mapper.Map<CategoriaDTO>(cat);

                return catDto;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar categoria do banco de dados");
            }
        }
    }
}
