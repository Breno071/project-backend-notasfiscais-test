using Data.Data;
using DomainApplication.DTOs;
using DomainApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_backend_test.Controllers;
using System.Collections.Generic;

namespace NotasFiscais.Tests
{
    public class NotaFiscalControllerTests : Helper
    {

        [Fact]
        public void DeveBuscarAsNotasFiscaisERetornarUmArrayDeNotasFiscais()
        {
            //Arrange
            NotasFiscaisContext context = GetNotasFiscaisContext();
            NotaFiscalController controller = new NotaFiscalController(context);

            //Act
            var resultado = controller.BuscarNotasFiscais().GetAwaiter().GetResult();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<NotaFiscal>>>(resultado);
        }

        [Fact]
        public void DeveCriarUmaNotaFiscalNoBancoComSucesso()
        {
            //Arrange
            NotasFiscaisContext context = GetNotasFiscaisContext();
            NotaFiscalController controller = new NotaFiscalController(context);
            NotaFiscalDTO nf = new()
            {
                DataNf = DateTime.Now,
                CnpjDestinatarioNf = "42103176000119",
                CnpjEmissorNf = "30473631000199",
                NumeroNf = "123",
                ValorTotal = 23
            };

            //Act
            controller.CadastrarNotaFiscal(nf).GetAwaiter().GetResult();
            var notaFiscal = context.NotaFiscal.FirstOrDefault(x => x.NumeroNf == nf.NumeroNf);

            //Assert
            Assert.IsType<NotaFiscal>(notaFiscal);
        }

        [Fact]
        public void DeveCriarUmaNotaFiscalNoBancoComSucessoEBuscarPeloIdDaNotaFiscalGeradaNaControllerEdeveRetornarANotaFiscal()
        {
            //Arrange
            NotasFiscaisContext context = GetNotasFiscaisContext();
            NotaFiscalController controller = new NotaFiscalController(context);
            NotaFiscalDTO nf = new()
            {
                DataNf = DateTime.Now,
                CnpjDestinatarioNf = "42103176000119",
                CnpjEmissorNf = "30473631000199",
                NumeroNf = "1234",
                ValorTotal = 23
            };

            //Act
            var notaFiscal = controller.CadastrarNotaFiscal(nf).GetAwaiter().GetResult();
            if (notaFiscal.Value is not null)
            {
                controller.BuscarNotaFiscal(notaFiscal.Value.NotaFiscalId).GetAwaiter().GetResult();

                //Assert
                Assert.IsType<NotaFiscal>(notaFiscal.Value);
            }
        }

        [Fact]
        public void DeveCadastrarUmaNotaFiscalEDepoisRemoverElaDoBanco()
        {
            //Arrange
            NotasFiscaisContext context = GetNotasFiscaisContext();
            NotaFiscalController controller = new NotaFiscalController(context);
            NotaFiscalDTO nf = new()
            {
                DataNf = DateTime.Now,
                CnpjDestinatarioNf = "42103176000119",
                CnpjEmissorNf = "30473631000199",
                NumeroNf = "12345",
                ValorTotal = 23
            };

            //Act
            var notaFiscal = controller.CadastrarNotaFiscal(nf).GetAwaiter().GetResult();
            if (notaFiscal.Value is not null)
            {
                controller.DeleteNotaFiscal(notaFiscal.Value.NotaFiscalId).GetAwaiter().GetResult();

                var nfDeletada = context.NotaFiscal.FirstOrDefault(x => x.NotaFiscalId == notaFiscal.Value.NotaFiscalId);

                //Assert
                Assert.Null(nfDeletada);
            }
        }
    }
}