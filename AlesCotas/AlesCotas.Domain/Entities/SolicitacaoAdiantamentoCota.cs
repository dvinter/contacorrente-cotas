using AlesCotas.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Entities
{
    public class SolicitacaoAdiantamentoCota : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public DateTime MesReferente { get; set; }
        public int GabineteId { get; set; }
        
        [ForeignKey("GabineteId")]
        public Gabinete? Gabinete { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataResposta { get; set; }
        public string? Mensagem { get; set; }

        public int? UsuarioRespostaId { get; set; }
        public Usuario? UsuarioResposta { get; set; }
    }
}
