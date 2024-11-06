using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Controllers;
[Route("api/categorias")]
[ApiController]
public class CategoriaController : ControllerBase
{
	private readonly ServicoCategoria ServicoCategoria;
	private IMapper mapeador;

	public CategoriaController(ServicoCategoria servicoCategoria, IMapper mapeador)
	{
		ServicoCategoria = servicoCategoria;
		this.mapeador = mapeador;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var resultado = await ServicoCategoria.SelecionarTodosAsync();

		if (resultado.IsFailed)
		{
			return StatusCode(500);
		}

		return Ok(resultado.Value);
	}

	[HttpPost]
	public async Task<IActionResult> Post(InserirCategoriaViewModel categoriaVm)
	{
		var categoria = mapeador.Map<Categoria>(categoriaVm);

		var resultado = await ServicoCategoria.InserirAsync(categoria);

		if (resultado.IsFailed)
		{
			return BadRequest(resultado.Errors);
		}


		return Ok(categoriaVm);
	}


}
