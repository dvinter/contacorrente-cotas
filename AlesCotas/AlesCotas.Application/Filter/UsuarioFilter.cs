using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Filtros.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter
{
    public class UsuarioFilter : BaseEntityFilter
    {
        public EFiltroUsuarioPerfil Perfil { get; set; }
    }
}
