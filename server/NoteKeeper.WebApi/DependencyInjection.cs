﻿using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NoteKeeper.Aplicacao.ModuloCategoria;
using NoteKeeper.Aplicacao.ModuloNota;
using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.Dominio.ModuloNota;
using NoteKeeper.Infra.Orm.Compartilhado;
using NoteKeeper.Infra.Orm.ModuloCategoria;
using NoteKeeper.Infra.Orm.ModuloNota;
using NoteKeeper.WebApi.Config.Mapping;
using NoteKeeper.WebApi.Config.Mapping.Action;
using NoteKeeper.WebApi.Filters;
using Serilog;

namespace NoteKeeper.WebApi;

public static class DependencyInjection
{
	public static void ConfigureDbContext(this IServiceCollection services, IConfiguration config)
	{
		var connectionString = config["SQL_SERVER_CONNECTION_STRING"];

		services.AddDbContext<IContextoPersistencia, NoteKeeperDbContext>(optionsBuilder =>
		{
			optionsBuilder.UseSqlServer(connectionString, dbOptions =>
			{
				dbOptions.EnableRetryOnFailure();
			});
		});
	}

	public static void ConfigureCoreServices(this IServiceCollection services)
	{
		services.AddScoped<IRepositorioCategoria, RepositorioCategoriaOrm>();
		services.AddScoped<ServicoCategoria>();

		services.AddScoped<IRepositorioNota, RepositorioNotaOrm>();
		services.AddScoped<ServicoNota>();
	}

	public static void ConfigureAutoMapper(this IServiceCollection services)
	{
		services.AddScoped<ConfigurarCategoriaMappingAction>();
		services.AddAutoMapper(config =>
		{
			config.AddProfile<CategoriaProfile>();
			config.AddProfile<NotaProfile>();
		});
	}

	public static void ConfigureCors(this IServiceCollection services, string politicaCors)
	{
		services.AddCors(options =>
		{
			options.AddPolicy(name: politicaCors, policy =>
			{
				policy
				.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
			});
		});
	}

	public static void ConfigureControllersWithFilters(this IServiceCollection services)
	{
		services.AddControllers(options =>
		{
			options.Filters.Add<ResponseWrapperFilter>();
		});
	}

	public static void ConfigureSerilog(this IServiceCollection services, ILoggingBuilder logging, IConfiguration config)
	{
		Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.Enrich.WithClientIp()
			.Enrich.WithMachineName()
			.Enrich.WithThreadId()
			.WriteTo.Console()
			.WriteTo.NewRelicLogs(
			endpointUrl: "https://log-api.newrelic.com/log/v1",
			applicationName: "note-keeper-api-bit",
			licenseKey: config["SERILOG_LICENSE_KEY"])
			.CreateLogger();

		logging.ClearProviders();

		services.AddLogging(builder => builder.AddSerilog(dispose: true));
	}

	public static void ConfigureSwaggerAuthorization(this IServiceCollection services)
	{
		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Title = "note-keeper-api", Version = "v1" });

			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Name = "Authorizarion",
				Description = "Por favor informe o token no padrão {Bearer token}",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string []{}
				}
			});
		});
	}
}