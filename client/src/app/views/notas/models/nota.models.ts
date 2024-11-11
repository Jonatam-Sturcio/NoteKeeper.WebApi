import { ListarCategoriaViewModel } from "../../categorias/models/categoria.models";

export interface InserirNotaViewModel {
  titulo: string;
  conteudo: string;
  arquivada: boolean;

  categoriaId: string;
}

export interface NotaInseridaViewModel {
  titulo: string;
  conteudo: string;
  arquivada: boolean;

  categoriaId: string;
}

export interface EditarNotaViewModel {
  titulo: string;
  conteudo: string;
  arquivada: boolean;

  categoriaId: string;
}

export interface NotaEditadaViewModel {
  titulo: string;
  conteudo: string;
  arquivada: boolean;

  categoriaId: string;
}

export interface ListarNotaViewModel {
  id: string;
  titulo: string;
  arquivada: boolean;

  categoria: ListarCategoriaViewModel;
}

export interface VisualizarNotaViewModel {
  id: string;
  titulo: string;
  conteudo: string;
  arquivada: boolean;

  categoria: ListarCategoriaViewModel;
}

export interface NotaExcluidaViewModel {}
