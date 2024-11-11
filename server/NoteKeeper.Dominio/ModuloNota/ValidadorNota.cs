using FluentValidation;

namespace NoteKeeper.Dominio.ModuloNota;

public class ValidadorNota : AbstractValidator<Nota>
{
	public ValidadorNota()
	{
		RuleFor(x => x.Titulo).NotEmpty().WithMessage("O título é obrigatório")
			.MaximumLength(30).WithMessage("O titulo deve conter no maximo 3 caracteres")
			.MinimumLength(3).WithMessage("O titulo deve conter no minimo 3 caracteres");

		RuleFor(x => x.Conteudo)
			.MaximumLength(100).WithMessage("O conteudo deve conter no maximo 100 caracteres");
	}
}