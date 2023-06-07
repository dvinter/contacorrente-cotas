using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Enum
{
    public enum ETipoAleracoes
    {
        [Description("Nenhuma Alteração")]
        Nenhuma = 0,
        [Description("Aumentar Quantidades")]
        AumentarQuantidades = 1,
        [Description("Diminuir Quantidades")]
        DiminuirQuantidades = 2,
        [Description("Alteração Total")]
        Total = 9
    }
}
