using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainApplication.Models;

namespace Data.Data
{
    public class NotasFiscaisContext : DbContext
    {
        public NotasFiscaisContext(DbContextOptions<NotasFiscaisContext> options)
            : base(options)
        {
        }

        public DbSet<NotaFiscal> NotaFiscal { get; set; } = default!;
    }
}
