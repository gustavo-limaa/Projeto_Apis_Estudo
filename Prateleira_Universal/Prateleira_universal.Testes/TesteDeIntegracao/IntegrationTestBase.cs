using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prateleira_Universal.Testes.TesteDeIntegracao
{
    public class IntegrationTestBase
    {
        protected AppDbContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Banco único por teste
                .Options;

            return new AppDbContext(options);
        }
    }
}