using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Filtros.Enum
{
    public enum EFiltroSolicitacaoStatus
    {
        [Description("Todos")]
        Todos = 0,

        [Description("Pendente")]
        Pendente = 1,

        [Description("Aprovado")]
        Aprovado = 2,

        [Description("Cancelado")]
        Cancelado = 3,

        [Description("Recusada")]
        Recusada = 4
    }
}
