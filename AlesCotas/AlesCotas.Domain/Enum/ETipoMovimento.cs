using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Enum
{
    public enum ETipoMovimento
    {
        [Description("Cota Parlamentar")]
        Cota  = 100,
        [Display(Name = "Item de Ajuste")]
        [Description("Item de Ajuste")]
        ItemAjuste = 200,
        [Description("Solicitação")]
        Solicitacao = 300,
        //[Description("Transferência de Saldo")]
        //TransferenciaSaldo = 400
    }
}
