using Newtonsoft.Json;

namespace AlesCotas.Presentation.Models
{
    public class CotacaoDolarModel
    {
        public decimal Cotacao { get; set; }
        public DateTime Data { get; set; }
        public string DataStr { get => this.Data.ToShortDateString(); }
    }


    public class CotacaoBCB
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<CotacaoItem> value { get; set; }
    }

    public class CotacaoItem
    {
        public decimal paridadeCompra { get; set; }
        public decimal paridadeVenda { get; set; }
        public decimal cotacaoCompra { get; set; }
        public decimal cotacaoVenda { get; set; }
        public DateTime dataHoraCotacao { get; set; }
        public string tipoBoletim { get; set; }
    }
}
