using Microsoft.AspNetCore.Mvc;
using RedSocialNetCore.Repositories;

namespace RedSocialNetCore.Controllers
{
    public class BuscadorController : Controller
    {
        private readonly RepositoryBuscador repo;

        public BuscadorController(RepositoryBuscador repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BuscarUsuarios(string query)
        {
            var usuariosEncontrados = repo.BuscarUsuarios(query);
            return View("Index", usuariosEncontrados);
        }
    }
}
