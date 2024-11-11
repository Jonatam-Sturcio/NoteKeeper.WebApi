using FluentValidation;
using NoteKeeper.Dominio.ModuloCategoria;

public class ValidadorCategoria : AbstractValidator<Categoria>
{
	public ValidadorCategoria()
	{
		RuleFor(x => x.Titulo).NotEmpty().WithMessage("O título é obrigatório")
			.MinimumLength(3).WithMessage("O título deve conter no minimo 3 caracteres")
			.MaximumLength(30).WithMessage("O título deve conter no máximo 30 caracteres");
	}
}