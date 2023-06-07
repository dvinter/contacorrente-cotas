using AlesCotas.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Models
{
    public class NovaSolicitacaoModel
    {
        public Solicitacao Solicitacao { get; set; }

        [Required(ErrorMessage ="Selecione pelo menos um gabinete")]
        public List<int> Gabinetes { get; set; }
        public int ProdutoTipoId { get; set; }

        public IFormFile[] FileAnexo { get; set; }


        public DateTime? RecorrenciaInicio { get; set; }
        public DateTime? RecorrenciaFim { get; set; }

    }

}
