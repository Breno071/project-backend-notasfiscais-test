using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DomainApplication.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Data.Data;
using DomainApplication.DTOs;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using System.Net;

namespace project_backend_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaFiscalController : ControllerBase
    {
        private readonly NotasFiscaisContext _context;

        public NotaFiscalController(NotasFiscaisContext context)
        {
            _context = context;
        }

        // GET: api/NotaFiscal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaFiscal>>> BuscarNotasFiscais()
        {
            if (_context.NotaFiscal == null)
            {
                return NotFound();
            }
            return await _context.NotaFiscal.ToListAsync();
        }

        // GET: api/NotaFiscal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotaFiscal>> BuscarNotaFiscal(long id)
        {
            if (_context.NotaFiscal == null)
            {
                return NotFound();
            }
            var notaFiscal = await _context.NotaFiscal.FindAsync(id);

            if (notaFiscal == null)
            {
                return NotFound();
            }

            return notaFiscal;
        }

        // PUT: api/NotaFiscal/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarNotaFiscal(long id, NotaFiscal notaFiscal)
        {
            if (id != notaFiscal.NotaFiscalId)
            {
                return BadRequest();
            }

            _context.Entry(notaFiscal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaFiscalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NotaFiscal
        [HttpPost]
        public async Task<ActionResult<NotaFiscal>> CadastrarNotaFiscal(NotaFiscalDTO notaFiscalDTO)
        {
            if (_context.NotaFiscal == null)
            {
                return Problem("Entity set 'NotasFiscaisContext.NotaFiscal'  is null.");
            }

            if (_context.NotaFiscal.Any(x => x.NumeroNf == notaFiscalDTO.NumeroNf))
                return BadRequest("Já existe uma Nota Fiscal com esse número");

            var notafiscal = new NotaFiscal
            {
                ValorTotal = notaFiscalDTO.ValorTotal,
                DataNf = notaFiscalDTO.DataNf,
                CnpjDestinatarioNf = notaFiscalDTO.CnpjDestinatarioNf,
                CnpjEmissorNf = notaFiscalDTO.CnpjEmissorNf,
                NumeroNf = notaFiscalDTO.NumeroNf
            };

            string outputFile = $"nf{notafiscal.NumeroNf}.pdf";

            notafiscal.FilePath = GeneratePDF(notafiscal, outputFile);

            _context.Add(notafiscal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(NotaFiscalController.BuscarNotaFiscal), new { id = notafiscal.NotaFiscalId }, notafiscal);
        }

        private string GeneratePDF(NotaFiscal notaFiscal, string outputFile)
        {
            FileInfo os = new FileInfo(outputFile);
            using (PdfWriter writer = new(os))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document document = new Document(pdf))
            {
                // Define cores personalizadas
                Color primaryColor = new DeviceRgb(0, 102, 204);

                // Adiciona um cabeçalho colorido
                Div header = new Div()
                    .SetBackgroundColor(primaryColor)
                    .SetHeight(60)
                    .SetMarginBottom(20);
                Paragraph headerText = new Paragraph("Nota Fiscal")
                    .SetFontSize(24)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(10);
                header.Add(headerText);
                document.Add(header);

                // Adiciona os detalhes da nota fiscal
                document.Add(new Paragraph("Número da NF: " + notaFiscal.NumeroNf)
                    .SetMarginBottom(10));
                document.Add(new Paragraph("Valor Total: " + notaFiscal.ValorTotal.ToString("C"))
                    .SetMarginBottom(10));
                document.Add(new Paragraph("Data da NF: " + notaFiscal.DataNf.ToShortDateString())
                    .SetMarginBottom(10));
                document.Add(new Paragraph("CNPJ do Emissor: " + notaFiscal.CnpjEmissorNf)
                    .SetMarginBottom(10));
                document.Add(new Paragraph("CNPJ do Destinatário: " + notaFiscal.CnpjDestinatarioNf)
                    .SetMarginBottom(10));

                return outputFile;
            }
        }

        // DELETE: api/NotaFiscal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotaFiscal(long id)
        {
            if (_context.NotaFiscal == null)
            {
                return NotFound();
            }
            var notaFiscal = await _context.NotaFiscal.FindAsync(id);
            if (notaFiscal == null)
            {
                return NotFound();
            }

            _context.NotaFiscal.Remove(notaFiscal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("download")]
        public FileContentResult DownloadPDF(NotaFiscal notaFiscal)
        {
            HttpResponseMessage result = new HttpResponseMessage();
            if (System.IO.File.Exists(notaFiscal.FilePath))
            {
                //Cria um http response para enviar como arquivo
                result.StatusCode = HttpStatusCode.OK;

                return File(System.IO.File.ReadAllBytes(notaFiscal.FilePath), "application/octet-stream", notaFiscal.FilePath, true); // returns a FileStreamResult
            }
            throw new Exception("Arquivo não encontrado");
        }

        private bool NotaFiscalExists(long id)
        {
            return (_context.NotaFiscal?.Any(e => e.NotaFiscalId == id)).GetValueOrDefault();
        }
    }
}
