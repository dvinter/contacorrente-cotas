
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

namespace AlesCotas.Domain.Entities
{

    public class Usuario : IdentityUser<int>, IBaseEntity
    {
        public string Matricula { get => this.UserName; set => this.UserName = value; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int? GabineteId { get; set; }

        [ForeignKey("GabineteId")]
        public Gabinete? Gabinete { get; set; }
        public string? Setor { get; set; }
        public string? ResetPassToken { get; set; }
        public DateTime? ResetPassTokenExpire { get; set; }
        public int Attempts { get; set; }

        [NotMapped]
        public EUsuarioPerfil? Role { get; set; }
    }

    public class UsuarioRoles
    {
        public int UsuarioId { get; set; }
        public int RoleId { get; set; }
        public EUsuarioPerfil Perfil { get; set; }
        public string PerfilNome { get => this.Perfil.GetEnumDescription(); }
    }

    public class Roles : IdentityRole<int>
    {
        EUsuarioPerfil _perfil;
        public EUsuarioPerfil Perfil { get => _perfil; set { _perfil = value; base.Name = ((int)_perfil).ToString(); } }
        public override string Name { get => ((int)this.Perfil).ToString(); set => this.Perfil = (EUsuarioPerfil)(int.TryParse(value, out int v) ? v : 4); }

         
    }
}
