namespace NoteKeeper.WebApi.ViewModels;

public class RegistrarUsuarioViewModel
{
	public string UserName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
}

public class AutenticarUsuarioViewModel
{
	public string UserName { get; set; }
	public string Password { get; set; }
}

public class TokenViewModel
{
	public string Chave { get; set; }
	public DateTime DataExpiracao { get; set; }
	public UsuarioTokenViewModel Usuario { get; set; }
}

public class UsuarioTokenViewModel
{
	public Guid Id { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
}