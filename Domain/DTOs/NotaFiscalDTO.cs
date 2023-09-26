using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainApplication.DTOs
{
    public class NotaFiscalDTO
    {
        public string NumeroNf { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public DateTime DataNf { get; set; }
        public string CnpjEmissorNf { get; set; } = string.Empty;
        public string CnpjDestinatarioNf { get; set; } = string.Empty;
    }
}
