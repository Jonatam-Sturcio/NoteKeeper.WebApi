using NoteKeeper.Dominio.ModuloAutenticacao;
using System.Security.Claims;

namespace NoteKeeper.WebApi.Identity;

public class ApiTenantProvider : ITenantProvider
{
	private readonly IHttpContextAccessor contextAcessor;

	public ApiTenantProvider(IHttpContextAccessor contextAcessor)
	{
		this.contextAcessor = contextAcessor;
	}

	public Guid? UsuarioId
	{
		get
		{
			var claimId = contextAcessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

			if (claimId == null) return null;

			return Guid.Parse(claimId.Value);
		}
	}
}