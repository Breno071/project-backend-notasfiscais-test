using Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotasFiscais.Tests
{
    public class Helper
    {
        protected static NotasFiscaisContext GetNotasFiscaisContext()
        {
            var options = new DbContextOptionsBuilder<NotasFiscaisContext>()
                .UseInMemoryDatabase(databaseName: "InMemory")
                .Options;
            return new NotasFiscaisContext(options);
        }
    }
}
