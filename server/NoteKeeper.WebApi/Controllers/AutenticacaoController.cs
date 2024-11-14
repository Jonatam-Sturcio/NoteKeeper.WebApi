using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteKeeper.Aplicacao.ModuloAutenticacao;
using NoteKeeper.Dominio.ModuloAutenticacao;
using NoteKeeper.WebApi.Identity;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
	private readonly ServicoAutenticacao servicoAutenticacao;
	private readonly IMapper mapeador;
	private readonly JsonWebTokenProvider jsonWebTokenProvider;

	public AutenticacaoController(ServicoAutenticacao servicoAutenticacao, IMapper mapeador, JsonWebTokenProvider jsonWebTokenProvider)
	{
		this.servicoAutenticacao = servicoAutenticacao;
		this.mapeador = mapeador;
		this.jsonWebTokenProvider = jsonWebTokenProvider;
	}

	[HttpPost("registrar")]
	public async Task<IActionResult> Registrar(RegistrarUsuarioViewModel viewModel)
	{
		var usuario = mapeador.Map<Usuario>(viewModel);

		var usuarioResult = await servicoAutenticacao.RegistrarAsync(usuario, viewModel.Password);

		if (usuarioResult.IsFailed)
			return BadRequest(usuarioResult.Errors);

		var tokenViewModel = jsonWebTokenProvider.GerarTokenAcesso(usuario);

		return Ok(tokenViewModel);
	}

	[HttpPost("autenticar")]
	public async Task<IActionResult> Autenticar(AutenticarUsuarioViewModel viewModel)
	{
		var usuarioResult = await servicoAutenticacao.AutenticarAsync(viewModel.UserName, viewModel.Password);

		if (usuarioResult.IsFailed)
			return BadRequest(usuarioResult.Errors);

		var usuario = usuarioResult.Value;

		var tokenViewModel = jsonWebTokenProvider.GerarTokenAcesso(usuario);

		return Ok(tokenViewModel);
	}

	[HttpPost("sair")]
	[Authorize]
	public async Task<IActionResult> Sair()
	{
		await servicoAutenticacao.Sair();

		return Ok();
	}
}