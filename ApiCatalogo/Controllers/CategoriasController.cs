using ApiCatalogo.Context;
using ApiCatalogo.DTOs;
using ApiCatalogo.Models;
using ApiCatalogo.Pagination;
using ApiCatalogo.Repository;
using ApiCatalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            try
            {
                var categorias = await _uow.CategoriaRepository.GetCategoriasProdutos();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDTO;
            } catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter as categorias do banco de dados");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] QueryStringParameters categoriasParameters)
        {
            try
            {
                var categorias = await _uow.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

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
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var cat = await _uow.CategoriaRepository.GetById(cat => cat.CategoriaId == id);

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
        public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _uow.CategoriaRepository.Add(categoria);
                await _uow.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return CreatedAtRoute("ObterCategoria", new { id = categoriaDTO.CategoriaId }, categoriaDTO);
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao inserir nova categoria no banco de dados");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest($"Não foi possível atualizar a categoria com id={id}");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uow.CategoriaRepository.Update(categoria);
                await _uow.Commit();
                return Ok($"Categoria com id={id} foi atualizada com sucesso");
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao alterar categoria no banco de dados");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var cat = await _uow.CategoriaRepository.GetById(cat => cat.CategoriaId == id);

                if (cat == null)
                {
                    return NotFound($"A categoria com id={id} não foi encontrada");
                }

                _uow.CategoriaRepository.Delete(cat);
                await _uow.Commit();

                var catDto = _mapper.Map<CategoriaDTO>(cat);

                return catDto;
            } catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar categoria do banco de dados");
            }
        }
    }
}
