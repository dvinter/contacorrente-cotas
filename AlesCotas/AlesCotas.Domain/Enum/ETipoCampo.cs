using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Enum
{
    public enum ETipoCampo
    {
        [Description("Texto Simples")]
        TextoSimples = 0,
        [Description("Texto Longo")]
        TextoLongo = 1,
        [Description("Número Inteiro")]
        NumeroInteiro =2,
        [Description("Número Decimal")]
        NumeroDecimal = 3,
        [Description("Data")]
        Data = 4,
        [Description("Data e Hora")]
        DataHora = 5,
        [Description("Sim ou Não")]
        SimNao = 6,
        [Description("Moeda")]
        Moeda  = 7,
        [Description("Lista Suspensa")]
        ListaSuspensa = 8,
        [Description("Lista Opções")]
        ListaOpcoes = 9,
        [Description("Intervalo Datas")]
        IntervaloDatas = 10,
        [Description("Intervalo Datas e Hora")]
        IntervaloDatasHoras = 11,
    }
}
