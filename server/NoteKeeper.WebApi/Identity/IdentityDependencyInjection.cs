using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NoteKeeper.Aplicacao.ModuloAutenticacao;
using NoteKeeper.Dominio.ModuloAutenticacao;
using NoteKeeper.Infra.Orm.Compartilhado;
using System.Text;

namespace NoteKeeper.WebApi.Identity;

public static class IdentityDependencyInjection
{
	public static void ConfigureIdentity(this IServiceCollection services)
	{
		services.AddScoped<ServicoAutenticacao>();
		services.AddScoped<JsonWebTokenProvider>();
		services.AddScoped<ITenantProvider, ApiTenantProvider>();

		services.AddIdentity<Usuario, Cargo>(options =>
		{
			options.User.RequireUniqueEmail = true;
		}).AddEntityFrameworkStores<NoteKeeperDbContext>()
		.AddDefaultTokenProviders();
	}

	public static void ConfigureJwt(this IServiceCollection services, IConfiguration config)
	{
		var chaveAssinatura = config["JWT_GENERATION_KEY"];
		var audienciaValida = config["JWT_AUDIENCE_DOMAIN"];

		if (chaveAssinatura == null)
			throw new ArgumentException("Não foi possivel obter a chave de assinatura de tokens");

		if (audienciaValida == null)
			throw new ArgumentException("Não foi possivel obter o dominio da audiencia dos tokens");

		var chaveEmBytes = Encoding.ASCII.GetBytes(chaveAssinatura);

		services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(x =>
		{
			x.RequireHttpsMetadata = true;
			x.SaveToken = true;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(chaveEmBytes),
				ValidAudience = audienciaValida,
				ValidIssuer = "NoteKeeper",
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateLifetime = true
			};
		});
	}
}