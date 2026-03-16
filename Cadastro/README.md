# 🚀 Sistema de Cadastro e Gestão de Livros (API)

Este projeto é uma API robusta desenvolvida em **C#** e **ASP.NET Core**, focada em boas práticas de arquitetura, segurança e performance. O sistema permite o gerenciamento completo de usuários e livros, incluindo um sistema de autenticação seguro.

## 🛠️ Tecnologias e Ferramentas
* **C# 12 / .NET 8**
* **Entity Framework Core** (ORM)
* **SQL Server** (Banco de dados)
* **Swagger** (Documentação da API)

## 🏗️ Padrões de Desenvolvimento (Boas Práticas)
Durante o desenvolvimento, apliquei conceitos de nível profissional para garantir a qualidade do código:
* **Padronização de DTOs:** Uso de `Records` e DTOs específicos para entrada (`Create/Update`) e saída (`Response`), protegendo as entidades do banco de dados.
* **Segurança de Dados:** Validação de senhas fortes usando `Regular Expressions` (Regex) e comparação de campos com `[Compare]`.
* **Programação Assíncrona:** Implementação de métodos `async/await` para melhor performance e escalabilidade.
* **Nulabilidade:** Uso correto de *Nullable Reference Types* para evitar erros de referência nula (`NullReferenceException`).
* **Tratamento de Retornos:** Uso adequado dos códigos de status HTTP (`201 Created`, `204 NoContent`, `401 Unauthorized`, etc).

## 🔐 Funcionalidades de Segurança
- [x] Cadastro de usuário com validação de e-mail único.
- [x] Senha com requisitos: Letra maiúscula, minúscula, número e caractere especial.
- [x] Sistema de Login com mensagens genéricas para evitar exposição de dados.
- [x] Separação clara entre dados sensíveis e dados públicos.

## 🚀 Como rodar o projeto
1. Clone o repositório.
2. Atualize a `ConnectionString` no `appsettings.json`.
3. No console do gerenciador de pacotes, execute: `Update-Database`.
4. Aperte `F5` no Visual Studio para rodar.

5. ## 📍 Status do Projeto
> 🟢 **Ambiente de Desenvolvimento:** Atualmente o projeto foi desenvolvido para rodar **localmente**. 
> As instruções abaixo guiarão você na configuração do banco de dados local para execução via Visual Studio.
