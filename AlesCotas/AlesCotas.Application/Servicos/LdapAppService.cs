using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Model;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    public class LdapAppService : ILdapAppService
    {
        public readonly IConfiguration _configuration;

        public LdapAppService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public bool IsLdapEnabled() 
        { 
                var session = _configuration.GetSection("LDAP");
                if (session != null && !String.IsNullOrWhiteSpace(session["servidor"]))
                {
                    return true;
                }
                return false;
        
        }

        public async Task<ServidorViewModel> ConsultaUsuario(string matricula)
        {
            ServidorViewModel servidor = new();
            var session = _configuration.GetSection("LDAP");

            // Create a DirectoryEntry object to connect to the Active Directory
            DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + session["servidor"], session["usuario"], session["senha"]);

            DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);

            // Set the filter to retrieve all user objects
            directorySearcher.Filter = $"(&(objectClass=user)(samaccountname={matricula}))";

            // Perform the search and retrieve the result
            SearchResult searchResult = directorySearcher.FindOne();

            if (searchResult != null)
            {

                DirectoryEntry userEntry = searchResult.GetDirectoryEntry();
                servidor.NomeServidor = userEntry.Properties["displayname"].Value.ToString();
                servidor.MatriculaVinculo = userEntry.Properties["samaccountname"].Value.ToString();
                servidor.EmailServidor = userEntry.Properties["mail"].Value?.ToString();
                servidor.Setor = userEntry.Properties["title"].Value?.ToString();
                
                //StringBuilder stringBuilder= new StringBuilder();

                //foreach (string prop in userEntry.Properties.PropertyNames)
                //{
                //    stringBuilder.AppendLine(prop + ": " + userEntry.Properties[prop].Value?.ToString());

                //}

                //string final = stringBuilder.ToString();

                //servidor.NomeServidor = reader["nome_g"].ToString();
                //servidor.MatriculaVinculo = reader["matricula"].ToString();
                //servidor.Cargo = reader["cargo"].ToString();
                //servidor.Setor = reader["setor"].ToString();
                //servidor.Regime = reader["regime"].ToString();
                //servidor.PadraoSalarial = reader["padrao"].ToString();
                //servidor.CargaHoraria = reader["carga_horaria"].ToString();
                //servidor.DataAdmissao = reader["data_admissao"].ToString();
                //servidor.DataDemissao = reader["data_demissao"].ToString();
                //servidor.EmailServidor = reader["email_servidor"].ToString();
            }

            return servidor;
        }

        public async Task<bool> ValidarCredenciais(string login, string senha)
        {
            var session = _configuration.GetSection("LDAP");
            bool isValid = false;

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, session["servidor"], session["usuario"], session["senha"]))
            {
                // validate the credentials
                isValid = pc.ValidateCredentials(login, senha);
            }

            return isValid;
        }
    }
}
