using AlesCotas.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlesCotas.Application.Classes;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Application.Interfaces.Servicos.Base;

namespace AlesCotas.Application.Interfaces.Servicos
{
    public interface ICotaAppService : IAppServico<Cota,CotaDto, BasePeriodoFilter>
    {
        /// <summary>
        /// Obtém a data do último processamento de cotas
        /// </summary>
        /// <returns>Datetime</returns>
        Task<DateTime> UltimoProcessamentoCotas();
        /// <summary>
        /// Processa as cotas: carrega a cota ativa e processada inserindo os valores nas respectivas contas
        /// O Processo incluí a verificação se a cota já foi adiantada, e se é necessário somar ou subtrair a diferença do valor atualizado
        /// </summary>
        /// <returns></returns>
        Task<int> ProcessarCotas();

        Task<CotaDto> SelectCotaAtual(DateTime dataReferencia);
    }
}
