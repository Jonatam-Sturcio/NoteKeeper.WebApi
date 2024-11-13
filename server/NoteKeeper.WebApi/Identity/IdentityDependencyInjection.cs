using Microsoft.AspNetCore.Identity;
using NoteKeeper.Dominio.ModuloAutenticacao;
using NoteKeeper.Infra.Orm.Compartilhado;
using System.Runtime.CompilerServices;

namespace NoteKeeper.WebApi.Identity;

public static class IdentityDependencyInjection
{
	public static void ConfigureIdentity(this IServiceCollection services)
	{
		services.AddScoped<ITenantProvider, ApiTenantProvider>();

		services.AddIdentity<Usuario, Cargo>(options =>
		{
			options.User.RequireUniqueEmail = true;
		}).AddEntityFrameworkStores<NoteKeeperDbContext>()
		.AddDefaultTokenProviders();
	}
}