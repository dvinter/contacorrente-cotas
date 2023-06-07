(function(){
    const realCoinMask = document.querySelectorAll(".realCoinMask")
    realCoinMask.forEach((e, index) => {
        VMasker(realCoinMask[index]).maskMoney();
    })

    //id="masks" data-js="cpf" para usar só colocar dessa forma
    const masks = {
        cpf (value) {
          return value
            .replace(/\D+/g, '')
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d{1,2})/, '$1-$2')
            .replace(/(-\d{2})\d+?$/, '$1')
        },
      
        cnpj (value) {
          return value
            .replace(/\D+/g, '')
            .replace(/(\d{2})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{3})(\d)/, '$1/$2')
            .replace(/(\d{4})(\d)/, '$1-$2')
            .replace(/(-\d{2})\d+?$/, '$1')
        },
      
        phone (value) {
          return value
            .replace(/\D+/g, '')
            .replace(/(\d{2})(\d)/, '($1) $2')
            .replace(/(\d{4})(\d)/, '$1-$2')
            .replace(/(\d{4})-(\d)(\d{4})/, '$1$2-$3')
            .replace(/(-\d{4})\d+?$/, '$1')
        },
      
        phoneDDI (value) {
          return value
            .replace(/\D+/g, '')
            .replace(/(\d{2})(\d)/, '+$1 $2')
            .replace(/(\d{2})(\d)/, '($1) $2')
            .replace(/(\d{4})(\d)/, '$1-$2')
            .replace(/(\d{4})-(\d)(\d{4})/, '$1$2-$3')
            .replace(/(-\d{4})\d+?$/, '$1')
        },
      
        cep (value) {
          return value
            .replace(/\D+/g, '')
            .replace(/(\d{5})(\d)/, '$1-$2')
            .replace(/(-\d{3})\d+?$/, '$1')
        },
      
        pis (value) {
          return value
            .replace(/\D+/g, '')
            .replace(/(\d{3})(\d)/, '$1.$2')
            .replace(/(\d{5})(\d)/, '$1.$2')
            .replace(/(\d{5}\.)(\d{2})(\d)/, '$1$2-$3')
            .replace(/(-\d)\d+?$/, '$1')
        },
      }
      
      document.querySelectorAll('#masks').forEach($input => {
        const field = $input.dataset.js
      
        $input.addEventListener('input', e => {
          e.target.value = masks[field](e.target.value)
        }, false)
      })

      const brmoney = document.querySelectorAll("#brmoney");
      const brdate = document.querySelectorAll("#brdate");
      const brdatetime = document.querySelectorAll("#brdatetime");
      const brnumber = document.querySelectorAll("#brnumber");

      brmoney.forEach((item, index) => {
        item.innerHTML = parseFloat(item.innerText).toLocaleString("pt-BR", {
          style: "currency",
          currency: "BRL",
        });
      });

      brdate.forEach((item, index) => {

        let newdate = new Date(item.innerText);
        if(newdate=="Invalid Date"){
          item.innerHTML = '- - - -'
          return
        }
        item.innerHTML = newdate.toLocaleString("pt-BR", { dateStyle: "short" });
      });
      brdatetime.forEach((item, index) => {
        let newdate = new Date(item.innerText);
        item.innerHTML = newdate.toLocaleString("pt-BR", {
          dateStyle: "short",
          timeStyle: "short",
        });
      });
      brnumber.forEach((item, index) => {
        item.innerHTML = parseFloat(item.innerText).toLocaleString("pt-BR");
      });
})();

