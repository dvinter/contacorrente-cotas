using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Filtros.Enum
{
    public enum EFiltroUsuarioPerfil
    {
        [Description("Todos")]
        Todos = 0,
        [Description("Administrador")]
        Administrador = 1,
        [Description("Gestor")]
        Gestor = 2,
        [Description("Gabinete")]
        Gabinete = 3,
        [Description("Consulta")]
        Consulta = 4,
    }
}
