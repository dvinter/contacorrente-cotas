using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Enum
{
    public enum ETipoEventoLog
    {
        [Description("Sistema")]
        System = 0,
        [Description("Inserção")]
        Insert = 1,
        [Description("Atualização")]
        Update = 2,
        [Description("Exclusão")]
        Delete = 3,
        [Description("Processamento de Cotas")]
        ProcessamentoCotas = 4,
        [Description("Erro")]
        Error = 5,
    }
}
