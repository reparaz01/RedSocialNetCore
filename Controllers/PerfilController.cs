using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedSocialNetCore.Extensions;
using RedSocialNetCore.Helpers;
using RedSocialNetCore.Models;
using RedSocialNetCore.Repositories;

public class PerfilController : Controller
{
    private RepositoryPerfil repo;
    private HelperCryptography helper;

    public PerfilController(RepositoryPerfil repo)
    {
        this.repo = repo;
    }

    public IActionResult VerPerfil(string otherUser)
    {
        Usuario user = repo.GetUser(otherUser);
        HttpContext.Session.SetObject("OtherUser", user);


        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");
        var otherUserr = HttpContext.Session.GetObject<Usuario>("OtherUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        var publicaciones = repo.GetPublicacionesUsuario(otherUserr.Username);

        int publicacionesCount = publicaciones.Count();

        var seguidosCount = repo.GetSeguidosCount(otherUserr.Username);
        var seguidoresCount = repo.GetSeguidoresCount(otherUserr.Username);

        // Pasar los números a la vista
        ViewBag.PublicacionesCount = publicacionesCount;
        ViewBag.SeguidosCount = seguidosCount;
        ViewBag.SeguidoresCount = seguidoresCount;


        // Obtener el recuento de likes por publicación
        var likesPorPublicacion = new Dictionary<int, int>();
        foreach (var publicacion in publicaciones)
        {
            // Obtener el recuento de likes para cada publicación
            var likesCount = repo.GetLikes(publicacion.IdPublicacion);
            likesPorPublicacion.Add(publicacion.IdPublicacion, likesCount);
            // Verificar si el usuario ha likeado cada publicación
            publicacion.Likeado = repo.IsLiked(publicacion.IdPublicacion, currentUser.Username);
        }

        // Pasar el diccionario de recuento de likes a la vista
        ViewBag.LikesPorPublicacion = likesPorPublicacion;


        // En tu método VerPerfil del controlador PerfilController
        var isFollowing = repo.IsFollowing(currentUser.Username, otherUserr.Username);

        // Pasar la información a la vista
        ViewBag.IsFollowing = isFollowing;



        return View(publicaciones);

    }


    [HttpPost]
    public IActionResult Like(int idPublicacion)
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        repo.Like(idPublicacion, currentUser.Username);

        ViewBag.LikesPorPublicacion = ViewBag.LikesPorPublicacion + 1;

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Dislike(int idPublicacion)
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        repo.Dislike(idPublicacion, currentUser.Username);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ToggleLike(int idPublicacion, bool isLiked)
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        if (currentUser == null)
        {
            return Unauthorized(); // O maneja la autenticación de alguna manera adecuada
        }

        if (isLiked)
        {
            repo.Dislike(idPublicacion, currentUser.Username);
        }
        else
        {
            repo.Like(idPublicacion, currentUser.Username);
        }

        return Ok();
    }


    [HttpPost]
    public IActionResult Follow(string otherUser)
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        repo.Follow(currentUser.Username, otherUser);

        // Redireccionar de vuelta a la misma página después de seguir al usuario
        return RedirectToAction("VerPerfil", new { otherUser });
    }

    [HttpPost]
    public IActionResult Unfollow(string otherUser)
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        repo.Unfollow(currentUser.Username, otherUser);

        // Redireccionar de vuelta a la misma página después de dejar de seguir al usuario
        return RedirectToAction("VerPerfil", new { otherUser });
    }


    [HttpPost]
    public IActionResult EliminarPublicacion(int idPublicacion)
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        repo.EliminarPublicacion(idPublicacion, currentUser.Username);

        return RedirectToAction("VerPerfil", new { otherUser = currentUser.Username });
    }


}






