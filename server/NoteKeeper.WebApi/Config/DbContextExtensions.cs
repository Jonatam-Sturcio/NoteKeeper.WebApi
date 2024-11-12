using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Infra.Orm.Compartilhado;

namespace NoteKeeper.WebApi.Config;

public static class DbContextExtensions
{
	public static bool AutoMigrateDatabase(this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();

		var dbcontext = scope.ServiceProvider.GetRequiredService<IContextoPersistencia>();

		bool migracaoConcluida = false;

		if (dbcontext is NoteKeeperDbContext noteKeeperDbContext)
		{
			migracaoConcluida = MigradorBancoDados.AtualizarBancoDados(noteKeeperDbContext);
		}

		return migracaoConcluida;
	}
}