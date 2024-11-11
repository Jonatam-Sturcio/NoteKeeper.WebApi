using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NoteKeeper.WebApi.Filters;

public class ResponseWrapperFilter : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context)
	{
	}

	public void OnActionExecuted(ActionExecutedContext context)
	{
		if (context.Result is ObjectResult objectResult)
		{
			var valor = objectResult.Value;

			if (valor is List<IError> erros)
			{
				objectResult.Value = new
				{
					Sucesso = false,
					erros = erros.Select(err => err.Message)
				};
			}
			else
			{
				objectResult.Value = new
				{
					Sucesso = true,
					Dados = valor
				};
			}
		}
	}
}