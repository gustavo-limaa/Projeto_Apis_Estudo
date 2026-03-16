namespace Cadastro.Dtos.UsuarioDtos;

public record UsuarioUpdateDto(Guid Id,
   string Nome,
   string Email,
   string Senha,
   bool Ativo
   );