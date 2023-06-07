using AlesCotas.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AlesCotas.Application.Dto
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public decimal Valor { get; set; }
        public string TipoNome { get; set; }
        public int TipoId { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public string Unidade { get; set; }
    }
}
