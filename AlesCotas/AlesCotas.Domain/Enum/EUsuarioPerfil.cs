using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Enum
{
    public enum EUsuarioPerfil
    {
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
