using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedSocialNetCore.Repositories;
using RedSocialNetCore.Models;
using RedSocialNetCore.Extensions;
using System.Text;
using RedSocialNetCore.Helpers;

namespace RedSocialNetCore.Controllers
{
    public class InicioController : Controller
    {
        private RepositoryInicio repo;
        private HelperCryptography helper;

        public InicioController(RepositoryInicio repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            bool loginSuccess = await repo.LogInUserAsync(username, password);
            if (loginSuccess)
            {
                Usuario user = repo.GetUser(username);
                HttpContext.Session.SetObject("CurrentUser", user);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario o Contraseña Incorrectos.";
                return View();
            }
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(string username, string nombre, string password, string confirmPassword, string email)
        {
            if (repo.UsuarioExists(username))
            {
                ViewData["Mensaje"] = "El nombre de usuario ya está en uso.";
                return View();
            }

            if (!string.IsNullOrEmpty(email) && repo.EmailExists(email))
            {
                ViewData["Mensaje"] = "Este correo electrónico ya está en uso.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                return View();
            }


            // Llamar al método RegisterUserAsync con la contraseña tal como está
            await repo.RegisterUserAsync(username, password, email, nombre);

            Usuario user = repo.GetUser(username);
            HttpContext.Session.SetObject("CurrentUser", user);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Login");
        }
    }
}
