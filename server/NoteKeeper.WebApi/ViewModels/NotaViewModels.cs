namespace NoteKeeper.WebApi.ViewModels;

public class ListarNotaViewModel
{
	public Guid Id { get; set; }
	public string Titulo { get; set; }
	public bool Arquivada { get; set; }
	public ListarCategoriaViewModel Categoria { get; set; }
}

public class VisualizarNotaViewModel
{
	public Guid Id { get; set; }
	public string Titulo { get; set; }
	public string Conteudo { get; set; }
	public bool Arquivada { get; set; }
	public ListarCategoriaViewModel Categoria { get; set; }
}

public class FormsNotaViewModel
{
	public string Titulo { get; set; }
	public string Conteudo { get; set; }
	public bool Arquivada { get; set; }
	public Guid CategoriaId { get; set; }
}

public class InserirNotaViewModel : FormsNotaViewModel
{
}

public class EditarNotaViewModel : FormsNotaViewModel
{
}