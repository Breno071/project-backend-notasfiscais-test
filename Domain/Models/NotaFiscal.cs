using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainApplication.Models
{
    public class NotaFiscal
    {
        [Key]
        public long NotaFiscalId { get; set; }
        public string NumeroNf { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; } 
        public DateTime DataNf { get; set; } 
        public string CnpjEmissorNf { get; set; } = string.Empty;
        public string CnpjDestinatarioNf { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
    }
}
