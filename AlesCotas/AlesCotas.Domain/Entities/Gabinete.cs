using AlesCotas.Domain.Interfaces.Base;
using System.ComponentModel.DataAnnotations;


namespace AlesCotas.Domain.Entities
{
    public class Gabinete : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Ativo { get; set; }
    }
}
