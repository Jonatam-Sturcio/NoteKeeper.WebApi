using Microsoft.IdentityModel.Tokens;
using NoteKeeper.Dominio.ModuloAutenticacao;
using NoteKeeper.WebApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteKeeper.WebApi.Identity;

public class JsonWebTokenProvider
{
	private readonly string? chaveJWT;
	private readonly DateTime dataExpiracaoJWT;
	private string? audienciaValida;

	public JsonWebTokenProvider(IConfiguration config)
	{
		chaveJWT = config["JWT_GENERATION_KEY"];

		if (string.IsNullOrEmpty(chaveJWT))
			throw new ArgumentException("Chave de geração de tokens não configurada.");

		audienciaValida = config["JWT_AUDIENCE_DOMAIN"];

		if (string.IsNullOrEmpty(audienciaValida))
			throw new ArgumentException("Audiência válida para transmissão de tokens não configurada.");

		dataExpiracaoJWT = DateTime.Now.AddDays(3);
	}

	public TokenViewModel GerarTokenAcesso(Usuario usuario)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var chaveEmBytes = Encoding.ASCII.GetBytes(chaveJWT!);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = "NoteKeeper",
			Audience = audienciaValida,
			Subject = new ClaimsIdentity(new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, usuario.Email!),
				new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName!)
				}),
			Expires = dataExpiracaoJWT,

			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(chaveEmBytes),
				SecurityAlgorithms.HmacSha256Signature
			)
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);

		var tokenString = tokenHandler.WriteToken(token);

		return new TokenViewModel
		{
			Chave = tokenString,
			DataExpiracao = dataExpiracaoJWT,
			Usuario = new UsuarioTokenViewModel
			{
				Id = usuario.Id,
				UserName = usuario.UserName!,
				Email = usuario.Email!
			}
		};
	}
}