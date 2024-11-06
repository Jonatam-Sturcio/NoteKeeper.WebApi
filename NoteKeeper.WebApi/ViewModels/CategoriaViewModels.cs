namespace NoteKeeper.WebApi.ViewModels;

public class InserirCategoriaViewModel
{
	public string Titulo { get; set; }
}

public class EditarCategoriaViewModel
{
	public string Titulo { get; set; }
}

public class ListarCategoriaViewModel
{
	public Guid Id { get; set; }

	public string Titulo { get; set; }
}