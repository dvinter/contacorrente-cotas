//let cotacaoDolar = 0;

function GetCotacao() {
    return new Promise(function (resolve, reject) {
        //let totalSolicitacao = $('#totalSolicitacao').val();
        //totalSolicitacao = totalSolicitacao.replace(",", ".")
        //console.log("executando get dolar");
        //var today = new Date();

        //var dd = today.getDate();
        //var mm = today.getMonth() + 1;
        //var yyyy = today.getFullYear();
        //var dataAtual = "'" + mm + '-' + dd + '-' + yyyy + "'"

        //var url = "https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoMoedaDia(moeda='USD',dataCotacao=" + dataAtual + ")?%40moeda=%27"

        //$.ajax({
        //    url: url,
        //    type: 'GET',
        //    success: function (res) {
        //        resolve(res.value[0].cotacaoCompra);

        //        //console.log(res);

        //        //let cotacaoCompra = res.value[0].cotacaoCompra
        //        //cotacaoDolar = cotacaoCompra;
        //        //let totalDolar = totalSolicitacao / cotacaoCompra;

        //        ////console.log(totalDolar);

        //        //$('#cotacao').val(totalDolar.toFixed(2));
        //    },
        //    error: function (err) {
        //        reject(err);
        //    }
        //});


        $.ajax({
            type: 'GET',
            url: "../Solicitacoes/GetCotacaoDolar",
            contentType: false,
            processData: false,
            success: function (result) {
                resolve(result);
            },
            error: function (data) {
                console.error(data);
                //errorSwal("Erro ao processar cotas: " + data.responseText);
            }
        });

    });
}