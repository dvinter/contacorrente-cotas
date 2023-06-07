// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using AlesCotas.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace AlesCotas.Presentation.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;

        public ResetPasswordModel(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Nova senha")]
            [StringLength(100, ErrorMessage = "O campo {0} deve conter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirme sua senha")]
            [Compare("Password", ErrorMessage = "As senhas fornecidas devem ser as mesmas.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Código")]
            public string Code { get; set; }

        }

        public bool InvalidToken { get; set; } = false;
        public int Attempts { get; set; }

        public IActionResult OnGet(/*string code = null*/)
        {
            // Omit this code so as not to get an error message since the code no longer gets passed through here
            //if (code == null)
            //{
            //    return BadRequest("A code must be supplied for password reset.");
            //}
            //else
            //{
            //    Input = new InputModel
            //    {
            //        Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            //    };
            //    return Page();
            //}
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                if (error.Code == "InvalidToken")
                {
                    this.InvalidToken = true;
                    if(user.Attempts > 0)
                    {
                        this.Attempts = user.Attempts;
                        ModelState.AddModelError(string.Empty, "Código Inválido, tente novamente ou gere um novo código de recuperação.");
                    } else
                    {
                        this.Attempts = 0;
                        ModelState.AddModelError(string.Empty, "Código Inválido, gere um novo código de recuperação.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
