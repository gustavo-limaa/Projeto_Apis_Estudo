namespace Cadastro.Intefaces;

public interface IStatus
{
    bool Ativo { get; set; }

    void ativar() => Ativo = true;

    void desativar() => Ativo = false;
}