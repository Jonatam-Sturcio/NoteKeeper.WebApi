using AutoMapper;
using NoteKeeper.Dominio.ModuloCategoria;
using NoteKeeper.WebApi.ViewModels;

namespace NoteKeeper.WebApi.Config.Mapping;

public class CategoriaProfile : Profile
{
	public CategoriaProfile()
	{
		CreateMap<InserirCategoriaViewModel, Categoria>();
		CreateMap<EditarCategoriaViewModel, Categoria>();

		CreateMap<Categoria, ListarCategoriaViewModel>();
		CreateMap<Categoria, VisualizarCategoriaViewModel>();
	}
}
