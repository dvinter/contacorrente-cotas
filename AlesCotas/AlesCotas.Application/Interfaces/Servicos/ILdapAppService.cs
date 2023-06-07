using AlesCotas.Application.Classes;
using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Interfaces.Servicos
{
    public interface ILdapAppService
    {
        Task<ServidorViewModel> ConsultaUsuario(string matricula);

        Task<bool> ValidarCredenciais(string login, string senha);

        bool IsLdapEnabled();

    }

}
