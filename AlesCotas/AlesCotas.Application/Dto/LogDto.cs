using AlesCotas.Domain.Enum;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Dto
{
    public class LogDto
    {
        public DateTime DataEvento { get; set; }
        public ETipoEventoLog TipoEvento { get; set; }
        public string TipoEventoDescricao { get => this.TipoEvento.GetEnumDescription(); }
        public string Tabela { get; set; }
        public int Id { get; set; }
        public int? RefId { get; set; }
        public string Descricao { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioMatricula { get; set; }
        public string Ip { get; set; }
    }
}
