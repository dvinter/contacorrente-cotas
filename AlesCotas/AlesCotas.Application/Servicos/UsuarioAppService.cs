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
using Microsoft.AspNetCore.Identity;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using AlesCotas.Application.Filter;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Domain.Model;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace AlesCotas.Application.Servicos
{
    public class UsuarioAppService : AppServico<Usuario, UsuarioDto, UsuarioFilter>, IUsuarioAppService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _config;


        public UsuarioAppService(IMapper mapper, IUsuarioRepository repository, ICurrentUserManager currentUserManager, IConfiguration config) : base(mapper, repository, currentUserManager)
        {
            _usuarioRepository = repository;
            _config = config;
        }

        public override async Task<Usuario> Select(Expression<Func<Usuario, bool>> predicate = null)
        {
            var obj = await base.Select(predicate);
            var roles = await _usuarioRepository.GetRoles(obj);
            obj.Role = roles.Min();
            return obj;
        }

        public override IQueryable<UsuarioDto> QueryDto(Expression<Func<Usuario, bool>> predicate = null)
        {
            return this._repository.QueryAll(predicate)
                .SelectMany(b => _repository.Logs.Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) => new { x, l })
                .SelectMany(xl => _usuarioRepository.QueryRoles().Where(r => r.UsuarioId.Equals(xl.x.Id)).OrderBy(r => r.Perfil).Take(1).DefaultIfEmpty(),
                (x, r) => new UsuarioDto
                {
                    Matricula = x.x.Matricula,
                    Email = x.x.Email,
                    Nome = x.x.Nome,
                    Ativo = x.x.Ativo,
                    GabineteNome = x.x.Gabinete.Name,
                    Role = r.Perfil,
                    Setor = x.x.Setor,
                    Id = x.x.Id,
                    GabineteId = x.x.GabineteId,
                    UltimaAtualizacao = x.l.EventDate
                });
        }

        public override async Task<ListaPaginada<UsuarioDto>> ListPag(UsuarioFilter filter)
        {
            var query = QueryDto();

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Nome.Contains(filter.FilterText) || x.Matricula.Contains(filter.FilterText) || x.Email.Contains(filter.FilterText));
            }
            if (filter.Status != Domain.Filtros.Enum.EFiltroStatus.Todos)
            {
                query = query.Where(x => x.Ativo == (filter.Status == Domain.Filtros.Enum.EFiltroStatus.Ativo));
            }
            if (filter.Perfil != Domain.Filtros.Enum.EFiltroUsuarioPerfil.Todos)
            {
                query = query.Where(x => ((int)x.Role).Equals((int)filter.Perfil));
            }

            return await ToPagedList(query, filter.PageNumber, filter.PageSize);

        }

        public async Task<bool> SetStatus(int id, bool status)
        {
            var obj = await Select(x => x.Id.Equals(id));
            obj.Ativo = status;
            await this.Atualizar(obj);
            return true;
        }


        /// <summary>
        /// Busca o dados do Servidor no banco de dados ALES, pela matrícula
        /// </summary>
        /// <param name="txPesquisa">Matrícula do servidor</param>
        /// <returns></returns>
        public async Task<ServidorViewModel> GetServidor(string txPesquisa)
        {
            ServidorViewModel resultado = new ServidorViewModel();

            //Caso não seja informado nenhum texto para pesquisa
            //Força a pesquisa a retornar vazio otimizando o tempo de busca.
            if (string.IsNullOrEmpty(txPesquisa)) txPesquisa = "999999999999";

            //Ajusta o conteúdo para Maiúsculo equivalente aos dados do banco
            txPesquisa = txPesquisa.ToUpper();
            txPesquisa = txPesquisa.Replace(' ', '%');

            string sql = @"SELECT DISTINCT vw_fl_funcionario.codigo_cadfu,
                                        ( Substring(( vw_fl_funcionario.codigo_cadfu ), 1, 6) ) AS
                                        matricula,
                                        vw_fl_funcionario.nome_g,
                                        gg_codau.nome_codau                                     AS
                                        regime,
                                        vw_padrao.nomepad                                       AS
                                        padrao,
                                        Substring(( fl_horario.horas_diarias ), 2, 4)           AS
                                        carga_horaria,
                                        gg_profissoes.nome_profi                                AS cargo
                                        ,
                                        gg_local.nome_local
                                        AS setor,
                                        To_char(vw_fl_funcionario.data_admissao, 'DD/MM/YYYY')  AS
                                        data_admissao,
                                        To_char(vw_fl_funcionario.data_demissao, 'DD/MM/YYYY')  AS
                                        data_demissao,
                                        gg_geral.email_g_corporativo AS email_servidor
                        FROM   vw_fl_funcionario
                               INNER JOIN gg_local
                                       ON gg_local.codigo_local = vw_fl_funcionario.codigo_local
                               INNER JOIN gg_profissoes
                                       ON gg_profissoes.codigo_emp = vw_fl_funcionario.codigo_emp
                                          AND gg_profissoes.codigo_profi =
                                              vw_fl_funcionario.codigo_cargo
                               INNER JOIN gg_codau
                                       ON gg_codau.codigo_emp = vw_fl_funcionario.codigo_emp
                                          AND gg_codau.codigo_tipco =
                                              vw_fl_funcionario.codigo_tipco_regime
                                          AND gg_codau.codigo_codau =
                                              vw_fl_funcionario.codigo_codau_regime
                               INNER JOIN vw_padrao
                                       ON vw_padrao.codigo_emp = vw_fl_funcionario.codigo_emp
                                          AND vw_padrao.codigo_fil = vw_fl_funcionario.codigo_fil
                                          AND vw_padrao.codigo_padrao = vw_fl_funcionario.codigo_padrao
                               INNER JOIN fl_horario
                                       ON fl_horario.codigo_emp = vw_fl_funcionario.codigo_emp
                                          AND fl_horario.codigo_horar = vw_fl_funcionario.codigo_horar
                               INNER JOIN gg_geral on gg_geral.codigo_g = vw_fl_funcionario.codigo_g and gg_geral.codigo_emp = vw_fl_funcionario.codigo_emp

                        WHERE  vw_fl_funcionario.data_demissao IS null and 	   
	                           vw_fl_funcionario.codigo_cadfu Like @txPesquisa
                        ORDER  BY data_admissao desc limit 1";


            try
            {
                NpgsqlConnection conexao = new NpgsqlConnection(_config.GetSection("ConnectionStrings").GetSection("postgres").Value);
                
                await conexao.OpenAsync();

                using (NpgsqlCommand command = new NpgsqlCommand(sql, conexao))
                {
                    //command.Parameters.Add("@LikeTxPesquisa", NpgsqlTypes.NpgsqlDbType.Varchar);
                    command.Parameters.Add("@txPesquisa", NpgsqlTypes.NpgsqlDbType.Varchar);

                    //command.Parameters["@LikeTxPesquisa"].Value = "%" + txPesquisa + "%";
                    command.Parameters["@txPesquisa"].Value = txPesquisa + "%";

                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            resultado.NomeServidor = reader["nome_g"].ToString();
                            resultado.MatriculaVinculo = reader["matricula"].ToString();
                            resultado.Cargo = reader["cargo"].ToString();
                            resultado.Setor = reader["setor"].ToString();
                            resultado.Regime = reader["regime"].ToString();
                            resultado.PadraoSalarial = reader["padrao"].ToString();
                            resultado.CargaHoraria = reader["carga_horaria"].ToString();
                            resultado.DataAdmissao = reader["data_admissao"].ToString();
                            resultado.DataDemissao = reader["data_demissao"].ToString();
                            resultado.EmailServidor = reader["email_servidor"].ToString();
                        }
                    }
                }
            }
            catch (NpgsqlException ex) 
            {
                if (ex.Message.Contains("Failed to connect"))
                {
                    resultado.erroMsg = ex.Message;
                    resultado.erroConexao = true;
                    return resultado;
                }
            }

            return resultado;
        }
    }
}
