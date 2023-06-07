using AlesCotas.Application.Dto;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Filtros;
using AutoMapper;

namespace AlesCotas.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            // Produto Tipo
            CreateMap<ProdutoTipo, ProdutoTipoDto>().ReverseMap();
            //CreateMap<ProdutoTipoFiltroDTO, ProdutoTipoFiltro>();

            // Produto
            //CreateMap<ProdutoCadastroDTO, Produto>();
            CreateMap<Produto, ProdutoDto>().ReverseMap();
            //CreateMap<ProdutoFiltroDTO, ProdutoFiltro>();

            // Solicitacao
            //CreateMap<SolicitacaoCadastroDTO, Solicitacao>();
            //CreateMap<SolicitacaoItemCadastroDTO, SolicitacaoItem>();
            CreateMap<Solicitacao, SolicitacaoDto>().ReverseMap();
            //CreateMap<SolicitacaoItem, SolicitacaoItemDto>().ReverseMap();
            //CreateMap<SolicitacaoFiltroDTO, SolicitacaoFiltro>();

            //CreateMap<SolicitacaoItemArquivoDTO, SolicitacaoItemArquivo>().ReverseMap();


            //Usuario
            //CreateMap<UsuarioFiltroDto, UsuarioFiltro>();
            //CreateMap<UsuarioFiltro , UsuarioFiltroDto>();
            CreateMap<UsuarioDto, Usuario>();
            //CreateMap<Usuario , UsuarioDto>().ForMember(p => p.GabineteName, opt => opt.MapFrom(src=> src.Gabinete.Nome));


            //Usuario
            //CreateMap<GabineteFiltroDto, GabineteFiltro>();
            //CreateMap<GabineteFiltro, GabineteFiltroDto>();
            CreateMap<GabineteDto, Gabinete>();
            CreateMap<Gabinete, GabineteDto>();



        }
    }
}
