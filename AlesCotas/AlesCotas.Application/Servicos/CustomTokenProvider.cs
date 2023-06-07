using AlesCotas.Domain.Entities;
using AlesCotas.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    public class CustomTokenProvider : IUserTwoFactorTokenProvider<Usuario>
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IConfiguration configuration;
        public CustomTokenProvider(ApplicationDbContext _applicationDbContext, IConfiguration _configuration)
        {
            applicationDbContext = _applicationDbContext;
            configuration = _configuration;
        }
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<Usuario> manager, Usuario user)
        {
            return Task.Factory.StartNew<bool>(() => false);
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<Usuario> manager, Usuario user)
        {
            var rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code = new string(Enumerable.Repeat(chars, 6).Select(s => s[rand.Next(s.Length)]).ToArray());
            var encryptionService = new EncryptionService(code, configuration.GetSection("EncryptionKeys")["DefaultKey"]);

            // Register to the database fields using appdbcontext
            var now = DateTime.Now;

            user.ResetPassToken = encryptionService.EncryptToken(encryptionService.Token, encryptionService.EncryptionKey);
            user.ResetPassTokenExpire = now.AddHours(6);
            user.Attempts = 5;

            await applicationDbContext.SaveChangesAsync();

            return code;

        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<Usuario> manager, Usuario user)
        {

            if (String.IsNullOrWhiteSpace(token))
                return false;

            // Check data in fields, then erase token
            var encryptionService = new EncryptionService();

            // check if code matches the one in the database AND if it isn't expired AND if the numbers of
            // allowed attempts is greater than 0
            if(user.ResetPassTokenExpire > DateTime.Now && user.Attempts > 0)
            {
                if (!String.IsNullOrWhiteSpace(user.ResetPassToken) && encryptionService.DecryptToken(user.ResetPassToken, configuration.GetSection("EncryptionKeys")["DefaultKey"]) == token)
                {
                    user.ResetPassToken = null;
                    user.Attempts = 0;
                    await applicationDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    // user.ResetPassToken = null;
                    user.Attempts = --user.Attempts;
                    await applicationDbContext.SaveChangesAsync();
                    return false;
                }
            } 
            else
            {
                user.ResetPassToken = null;
                await applicationDbContext.SaveChangesAsync();
                return false;
            }

            // Defults at true for testing purposes
            // return true;
        }
    }
}
