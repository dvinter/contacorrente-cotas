using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Enum
{
    public enum ENaturezaMovimento
    {
        [Description("Crédito")]
        Credito = 0,
        [Description("Débito")]
        Debito = 1
    }
}
