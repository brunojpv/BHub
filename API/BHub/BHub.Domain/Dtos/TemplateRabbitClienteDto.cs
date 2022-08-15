using System;

namespace BHub.Domain.Dtos
{
    public class TemplateRabbitClienteDto
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }

        public string Telefone { get; set; }

        public string Endereco { get; set; }

        public DateTime DataCadastro { get; set; }

        public decimal Faturamento { get; set; }
    }
}
