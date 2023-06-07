using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Dto
{
    
    public class SolicitacaoAdiantamentoCotaDto 
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMMM \\de yyyy}")]
        public DateTime MesReferente { get; set; }
        public int GabineteId { get; set; }
        public string GabineteNome { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataResposta { get; set; }
        public string StatusDescription { get => this.DataResposta.HasValue ? this.Mensagem : "Em Aberto"; }
        public int? UsuarioRespostaId { get; set; }
        public string? UsuarioRespostaNome { get; set; }
        public string? Mensagem { get; set; }
        [DataType(DataType.Date)]
        public DateTime? UltimaAtualizacao { get; set; }
    }
}
