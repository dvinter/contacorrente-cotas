
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Base;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Dto
{

    public class UsuarioDto 
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int? GabineteId { get; set; }
        public string GabineteNome { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public string? Setor { get; set; }
        public EUsuarioPerfil? Role { get; set; }
        public string RoleName { get => this.Role?.GetEnumDescription(); }
    }
}
