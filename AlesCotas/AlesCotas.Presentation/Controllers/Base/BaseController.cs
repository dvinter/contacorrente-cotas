using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Base;
using AlesCotas.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace AlesCotas.Presentation.Controllers.Base
{
    /// <summary>
    /// Controle abstrato genérico para todos os requests padrões.
    /// Caso haja necessidade, os métodos podem ser sobrescritos nas implementações deste.
    /// </summary>
    /// <typeparam name="TEntity">Entidade qual o controller irá trabalhar para os métodos de Inserção e Edição</typeparam>
    /// <typeparam name="TDto">DTO qual o controller irá trabalhar para os métodos de consulta</typeparam>
    /// <typeparam name="TFilter">Tipo de filtro que o controller irá receber/enviar para a view</typeparam>
    [Authorize]
    public abstract class BaseController<TEntity, TDto, TFilter> : Controller where TEntity : class, IBaseEntity where TDto : class where TFilter : IBaseFilter
    {
        protected readonly IAppServico<TEntity, TDto, TFilter> _appServico;
        protected readonly UserManager<Usuario> _userManager;
        public BaseController(IAppServico<TEntity,TDto, TFilter> appServico, UserManager<Usuario> userManager)
        {
            this._appServico = appServico;
            _userManager = userManager;
        }

        // GET: Controller
        public virtual async Task<IActionResult> Index(TFilter filter)
        {
            

            await LoadViewBagListMode(filter);
            var lista = await _appServico.ListPag(filter);
            return View(lista);
        }

        //Mantém a informação passada para exibir um toast após um redirect to action
        public virtual void VerificaTempDataToast()
        {
            if (TempData.ContainsKey("toastSuccess"))
                ViewBag.toast = new ToastModel { Success = TempData["toastSuccess"].ToString() };
            if (TempData.ContainsKey("toastError"))
                ViewBag.toast = new ToastModel { Error = TempData["toastError"].ToString() };
        }

        // GET: Controller/Details/5
        public virtual async Task<IActionResult> Details(int? id)
        {
            if (id == null || _appServico == null)
            {
                return NotFound();
            }

            var obj = await _appServico.Select(x => x.Id.Equals(id));

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        /// <summary>
        /// Carrega a view Create sem a validalção do view model e sem receber um objeto vazio
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IActionResult> New()
        {
            await LoadViewBagEditMode(null);
            var obj = Activator.CreateInstance<TEntity>();
            ViewBag.toast = null;
            return View("Create", obj);
           
        }
        /// <summary>
        /// Carrega a view Create sem a validalção do view model mas recebendo um objeto preenchido
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IActionResult> NewEntity(TEntity entity)
        {
            await LoadViewBagEditMode(entity);
            ModelState.Clear();
            ViewBag.toast = null;
            return View("Create", entity);

        }

        /// <summary>
        /// Carrega a view Create com a validalção do view model passando o objeto já preenchido
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IActionResult> Create(TEntity? entity = null)
        {
            await LoadViewBagEditMode(entity);
            return View(entity);
        }

        // POST: Gabinetes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Salvar os dados da view, se o ID for vazio (zero) é efetuado um insert, caso contrário, update.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Save(TEntity entity)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");
            try
            {
                if (ModelState.IsValid)
                {
                    if (entity.Id <= 0)
                    {
                        var result = await _appServico.Cadastrar(entity);
                        TempData.Add("toastSuccess", "Registro inserido com sucesso!");
                    }
                    else
                    {
                        var result = await _appServico.Atualizar(entity);
                        TempData.Add("toastSuccess", "Registro atualizado com sucesso!");
                    }

                    return RedirectToAction(nameof(Index));
                }
                await LoadViewBagEditMode(entity);
                return View(nameof(Create), entity);
            }
            catch (Exception ex)
            {
                await LoadViewBagEditMode(entity);
                ViewBag.toast = new ToastModel { Error = "Erro: " + ex.Message };
                return View(nameof(Create), entity);
            }
        }

        // GET: Gabinetes/Edit/5
        public virtual async Task<IActionResult> Edit(int? id)
        {
            if (id == null ||_appServico == null)
            {
                return NotFound();
            }

            var obj = await _appServico.Select(x=>x.Id.Equals(id));
            if (obj == null)
            {
                return NotFound();
            }
            await LoadViewBagEditMode(obj);


            if (!(await PodeEditar(obj)))
            {
                return Forbid();
            }


            return View(nameof(Create), obj);
        }


        public virtual async Task<bool> PodeEditar(TEntity entity)
        {
            await Task.CompletedTask;
            return true;
        }


        // POST: Gabinetes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> Edit(int id,  TEntity entity)
        {
            if (id != entity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entry = await _appServico.Atualizar(entity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await Exists(entity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        // GET: Gabinetes/Delete/5
        public virtual async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _appServico == null)
            {
                return NotFound();
            }

            var obj = await _appServico.SelectDto(m => m.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST: Gabinetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_appServico == null)
            {
                return Problem("'AppService'  is null.");
            }
          
            var result =  await _appServico.Apagar(id);
            

            return RedirectToAction(nameof(Index));
        }

        protected async Task<bool> Exists(int id)
        {
            return await _appServico.Count(e => e.Id == id) > 0;
        }
        protected virtual async Task LoadViewBagEditMode(TEntity entity = null,  bool cleanModelState = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            ViewBag.user = user;


            StringBuilder sbError = new StringBuilder();
            foreach (var item in ModelState)
            {
                if (ModelState[item.Key].ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                {
                    foreach (var erro in ModelState[item.Key].Errors)
                    {
                        sbError.AppendLine(erro.ErrorMessage);
                    }
                }
            }
            if (sbError.Length > 0)
            {
                ViewBag.toast = new ToastModel { Error = sbError.ToString() };
            }

            if (cleanModelState)
                ModelState.Clear();

        }

        protected virtual async Task LoadViewBagListMode(TFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            ViewBag.user = user;
            ViewBag.filtro = filter;
            VerificaTempDataToast();
        }



        

    }
}
