using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UsuariosApi.Authorization
{
    public class IdadeAuthorization : AuthorizationHandler<IdadeMinima>

    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdadeMinima requirement)
        {
            var dataNacimentoClaim = context.User.FindFirst(claim => claim.Type == ClaimTypes.DateOfBirth);
            if (dataNacimentoClaim == null)
            { return Task.CompletedTask; }

            var dataNacimento = 
                Convert.ToDateTime(dataNacimentoClaim.Value);

            var idadeUsuario = 
                DateTime.Today.Year - dataNacimento.Year;


            if (dataNacimento > 
                DateTime.Today.AddYears(-idadeUsuario))
            {
                idadeUsuario--;
            }


            if (idadeUsuario >= requirement.Idade)
            { context.Succeed(requirement); }


            return Task.CompletedTask;
        }
    }
}
