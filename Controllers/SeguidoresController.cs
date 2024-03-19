using Microsoft.AspNetCore.Mvc;
using RedSocialNetCore.Helpers;
using RedSocialNetCore.Repositories;

namespace RedSocialNetCore.Controllers
{
    public class SeguidoresController : Controller
    {
        private RepositorySeguidores repo;

        public SeguidoresController(RepositorySeguidores repo)
        {
            this.repo = repo;
        }

        public IActionResult VerSeguidores(string username)
        {
            ViewBag.Username = username;
            var seguidores = repo.GetSeguidores(username);
            return View(seguidores);
        }

        public IActionResult VerSeguidos(string username)
        {
            ViewBag.Username = username;
            var seguidos = repo.GetSeguidos(username);
            return View(seguidos);
        }
    }
}
