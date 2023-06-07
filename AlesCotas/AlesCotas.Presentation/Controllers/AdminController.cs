using AlesCotas.Application.Classes;
using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Enum;
using AlesCotas.Infra.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AlesCotas.Presentation.Auth;

namespace AlesCotas.Presentation.Controllers
{
    

    [Authorize]
    [ClaimRequirement(EUsuarioPerfil.Administrador)]
    public class AdminController : Controller
    {
    private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logs(BasePeriodoFilter filter)
        {
            ViewBag.filtro = filter;
            var query = _context.Logs.Include(x => x.Usuario).Select(x => new LogDto
            {
                DataEvento = x.EventDate,
                Descricao = x.Description,
                Id = x.Id,
                Ip = x.IpAddress,
                Tabela = x.Entity,
                TipoEvento = x.EventType,
                UsuarioMatricula = x.Usuario.Matricula,
                UsuarioNome = x.Usuario.Nome,
                RefId = x.RefId

            });
            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Descricao.Contains(filter.FilterText)
                    || x.UsuarioNome.Contains(filter.FilterText)
                    || x.UsuarioMatricula.Contains(filter.FilterText)
                    || x.Ip.Equals(filter.FilterText)
                    || (x.RefId.HasValue && x.RefId.ToString().Contains(filter.FilterText))
                    || x.Tabela.Contains(filter.FilterText)
                    );
            }

            if (filter.DataInicial.HasValue)
            {
                query = query.Where(x => x.DataEvento.Date >= filter.DataInicial.Value.Date);
            }
            if (filter.DataFinal.HasValue)
            {
                query = query.Where(x => x.DataEvento.Date <= filter.DataFinal.Value.Date);
            }

            query = query.OrderByDescending(x => x.Id);

            var list = await ListaPaginada<LogDto>.ToListaPaginadaAsync(query, filter.PageNumber, filter.PageSize);
            return View(list);
        }
    }
}
