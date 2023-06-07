using AlesCotas.Application.Dto;
using AlesCotas.Application.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Filtros;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlesCotas.Application.Interfaces.Servicos;
using Microsoft.EntityFrameworkCore;
using AlesCotas.Application.Classes;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using AlesCotas.Infra.Data.Repositories;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Domain.Enum;

namespace AlesCotas.Application.Servicos
{


    public class MovimentoContaAppService : AppServico<MovimentoConta, MovimentoContaDto, BaseEntityFilter>, IMovimentoContaAppService
    {
        public MovimentoContaAppService(IMapper mapper, IMovimentoContaRepository repository, ICurrentUserManager currentUserManager) : base(mapper, repository, currentUserManager)
        {
        }

        public override IQueryable<MovimentoContaDto> QueryDto(Expression<Func<MovimentoConta, bool>> predicate = null)
        {
            return this._repository.QueryAll(predicate).SelectMany(b => _repository.Logs.Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) => new MovimentoContaDto
                {
                    Id = x.Id,
                    Descricao = x.Descricao,
                    Tipo = x.Tipo,
                    GabineteId = x.GabineteId,
                    Gabinete = x.Gabinete,
                    Valor = x.Valor,
                    DataMovimento = x.DataMovimento,
                    UltimaAtualizacao = l.EventDate,
                     DataReferenciaCota = x.DataReferenciaCota
                });
        }

        public override async Task<ListaPaginada<MovimentoContaDto>> ListPag(BaseEntityFilter filter)
        {
            var query = QueryDto();

            var user = await _currentUserManager.GetCurrentUser();

            if (user.Role == EUsuarioPerfil.Gabinete)
            {
                query = query.Where(x => x.GabineteId.Equals(user.GabineteId));
            }

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Descricao.Contains(filter.FilterText) || x.Descricao.Contains(filter.FilterText));
            }
            
            return await ToPagedList(query, filter.PageNumber, filter.PageSize);
        }
        public override async Task<MovimentoConta> Atualizar(MovimentoConta entity)
        {
            var user = await _currentUserManager.GetCurrentUser();

            var currentDb = await _repository.QueryAll(x => x.Id.Equals(entity.Id)).AsNoTracking().FirstOrDefaultAsync();

            if (currentDb.Tipo != Domain.Enum.ETipoMovimento.ItemAjuste && user.Role != Domain.Enum.EUsuarioPerfil.Administrador)
                throw new Exception("Apenas Administradores podem alterar movimentos diferentes de Item de Ajuste");

            if (user.Role != Domain.Enum.EUsuarioPerfil.Gestor && user.Role != Domain.Enum.EUsuarioPerfil.Administrador)
                throw new Exception("Apenas Administradores e gestores podem alterar movimentos");


            return await base.Atualizar(entity);
        }
        public override async Task<MovimentoConta> Apagar(int id)
        {
            var user = await _currentUserManager.GetCurrentUser();

            var entity = await  _repository.QueryAll(x => x.Id.Equals(id)).AsNoTracking().FirstOrDefaultAsync();

            if (entity.Tipo != Domain.Enum.ETipoMovimento.ItemAjuste && user.Role != Domain.Enum.EUsuarioPerfil.Administrador)
                throw new Exception("Apenas Administradores podem alterar movimentos diferentes de Item de Ajuste");

            if (user.Role != Domain.Enum.EUsuarioPerfil.Gestor && user.Role != Domain.Enum.EUsuarioPerfil.Administrador)
                throw new Exception("Apenas Administradores e gestores podem alterar movimentos");


            return await base.Apagar(id);
        }
        public override async Task<MovimentoConta> Cadastrar(MovimentoConta entity)
        {
            var user = await _currentUserManager.GetCurrentUser();

            if (entity.Tipo != Domain.Enum.ETipoMovimento.ItemAjuste && user.Role != Domain.Enum.EUsuarioPerfil.Administrador)
                throw new Exception("Apenas Administradores podem inserir movimentos diferentes de Item de Ajuste");

            if (user.Role != Domain.Enum.EUsuarioPerfil.Gestor && user.Role != Domain.Enum.EUsuarioPerfil.Administrador)
                throw new Exception("Apenas Administradores e gestores podem inserir movimentos");

            return await base.Cadastrar(entity);
        }
       
    }
}