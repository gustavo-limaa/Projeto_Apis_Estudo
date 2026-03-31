using Cadastro.Dados;
using Cadastro.Dtos.UsuarioDtos;
using Cadastro.Intefaces;
using Cadastro.Modelos;
using Cadastro.Modelos.Mapper;
using Microsoft.EntityFrameworkCore; // OBRIGATÓRIO para métodos Async do banco

namespace Cadastro.Dados.Repositorios;

public class RepoUsuario : IRepositorioUsuario
{
    private readonly AppDbContext _context;

    public RepoUsuario(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> AdicionarAsync(Usuario usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario));
        }
        _context.Usuarios.Add(usuario);

        var salvar = _context.SaveChanges();

        if (salvar == 0)
        {
            throw new Exception("Não foi possível salvar o usuário.");
        }

        return usuario;
    }

    public async Task<Usuario> AtualizarAsync(Guid id, UsuarioUpdateDto dto)
    {
        if (id == Guid.Empty) { return null; }
        // 1. Busca o usuário que já existe no banco
        var usuarioExistente = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);

        if (usuarioExistente == null)
        {
            throw new Exception("Usuário não encontrado para atualização.");
        }

        // 2. USA O SEU MAPPER!
        // Ele vai validar o novo e-mail no construtor do ValueEmail automaticamente.
        usuarioExistente.UpdateEntity(dto);

        // 3. Salva as mudanças
        // O EF detecta que as propriedades do 'usuarioExistente' mudaram e gera o SQL UPDATE
        int linhasAfetadas = await _context.SaveChangesAsync();

        if (linhasAfetadas == 0)
        {
            // Se o usuário mandou os mesmos dados que já estavam lá, o EF pode retornar 0
            // pois não houve "mudança real" no banco.
            return usuarioExistente;
        }

        return usuarioExistente;
    }

    public async Task<Usuario> DeletarAsync(Guid id)
    {
        if (id == Guid.Empty) { return null; }
        var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        await _context.Usuarios.Where(u => u.Id == id).ExecuteDeleteAsync();

        if (usuarioExistente == null)
        {
            throw new Exception("Usuário não encontrado para exclusão.");
        }
        return usuarioExistente;
    }

    public async Task<IEnumerable<Usuario>> ListartodosAsync()
    {
        var usuarios = await _context.Usuarios.ToListAsync();

        if (usuarios == null || !usuarios.Any())
        {
            throw new Exception("Nenhum usuário encontrado.");
        }

        return usuarios;
    }

    public async Task<Usuario> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty) { return null; }

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado.");
        }
        return usuario;
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        // O EF Core vai lá na tabela de Usuarios e traz o primeiro que bater o e-mail
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.Valor == email);
    }
}