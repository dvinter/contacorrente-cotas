using AlesCotas.Domain.Interfaces.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlesCotas.Domain.Entities
{
    public class Produto : IBaseEntity
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Campo nome é obrigatório")]
        
        public string Name { get; set; }
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public decimal Valor { get; set; }
        
        [ForeignKey("TipoId")]
        public ProdutoTipo? Tipo { get; set; }
        public int TipoId { get; set; }
        public string Unidade { get; set; }



    }
}
