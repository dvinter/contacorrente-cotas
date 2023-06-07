using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Servicos;
using AlesCotas.Application.Servicos.Auth;
using AlesCotas.Infra.Data.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AlesCotas.Presentation.Worker
{
    /// <summary>
    /// É Executado em segundo plano para processar a fila de movimentos financeiros e a inserção de cotas.
    /// Toda vez que é inserido um movimento financeiro, por trigger o movimento é inserido em uma fila para processamento do fluxo da conta corrente.
    /// IFluxoContaAppService.ProcessarFila processa essa fila em um nível de isolamento elevado para manter a integridade do fluxo, garantindo que os movimentos sejam inseridos em série, calculando os respectivos saldos.
    /// ICotaAppService.ProcessarCotas verifica a última execução do processo, se ainda não foi processado no mês atual, é carrega a cota ativa e processada inserindo os valores nas respectivas contas
    /// O Processo incluí a verificação se a cota já foi adiantada, e se é necessário somar ou subtrair a diferença do valor atualizado.
    /// </summary>
    public class AlesWorker : BackgroundService
    {
        private readonly ILogger<AlesWorker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AlesWorker(ILogger<AlesWorker> logger, IServiceScopeFactory serviceScopeFactory) =>
        (_logger, _serviceScopeFactory) = (logger, serviceScopeFactory);

        public AlesWorker(ILogger<AlesWorker> logger) =>
            _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                //Escopo pra injeção de dependência dos serviços
                using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        var serviceCollection = new ServiceCollection();
                        
                        //serviceCollection.AddScoped<ICurrentUserManager, SystemCurrentUserManager>();

                        var provider  = serviceCollection.BuildServiceProvider();

                        
                        ICotaAppService _cotaAppService = scope.ServiceProvider.GetRequiredService<ICotaAppService>();
                        IFluxoContaAppService _fluxoContaAppService = scope.ServiceProvider.GetRequiredService<IFluxoContaAppService>();
                        ISolicitacaoAppService _solicitacaoAppService = scope.ServiceProvider.GetRequiredService<ISolicitacaoAppService>();


                        var dataUltimoProcessamento = await _cotaAppService.UltimoProcessamentoCotas(); 
                        var dataReferenciaHoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                        if (dataUltimoProcessamento < dataReferenciaHoje)
                        {
                            try
                            {
                                await _cotaAppService.ProcessarCotas();
                                _logger.LogInformation("Processamento de Cotas: {time}", DateTimeOffset.Now);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, ex.Message);
                            }
                        }

                        try
                        {
                            await _fluxoContaAppService.ProcessarFila();
                            _logger.LogInformation("Processamento de fila: {time}", DateTimeOffset.Now);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }


                        try
                        {
                            await _solicitacaoAppService.ProcessarAprovacaoAutomatica();
                            _logger.LogInformation("Processamento de Aprovações automáticas: {time}", DateTimeOffset.Now);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, ex.Message);
                        }


                        await Task.Delay(3000, stoppingToken);

                    }
                    finally
                    {
                        _logger.LogInformation(
                      "Finished scoped work, provider hash: {hash}.{nl}",
                      scope.ServiceProvider.GetHashCode(), Environment.NewLine);
                    }
                }
            }
        }
    }

}
   