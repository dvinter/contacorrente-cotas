using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Filtros.Enum
{
    public enum EFiltroTipoData
    {
        [Description("Data de Registro")]
        DataRegistro = 0,
        [Description("Data de Movimento")]
        DataMovimento = 1
    }
}
