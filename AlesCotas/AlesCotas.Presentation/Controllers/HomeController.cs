using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Model;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AlesCotas.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDashBoardAppService _dashBoardAppService;
        protected readonly UserManager<Usuario> _userManager;
        public HomeController(ILogger<HomeController> logger, IDashBoardAppService dashBoardAppService, UserManager<Usuario> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _dashBoardAppService = dashBoardAppService;
        }

        public async Task<IActionResult> Index(DashBoardFilter filter)
        {
            ViewBag.Filter = filter;
            ViewBag.gabinetes = await _dashBoardAppService.GetGabinetes();
            ViewBag.produtoTipos = await _dashBoardAppService.GetProdutoTipos();

            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            ViewBag.user = user;

            DashBoardViewModel model = await _dashBoardAppService.GetDashBoard(filter);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}