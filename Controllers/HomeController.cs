using Microsoft.AspNetCore.Mvc;
using RedSocialNetCore.Extensions;
using RedSocialNetCore.Models;
using RedSocialNetCore.Repositories;
using System.Collections.Generic;

public class HomeController : Controller
{
    private readonly RepositoryHome repo;

    public HomeController(RepositoryHome repo)
    {
        this.repo = repo;
    }

    public IActionResult Index()
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        HttpContext.Session.Remove("OtherUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        var publicaciones = repo.GetAllPublicacionesExceptoUsuario(currentUser.Username);

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

        return View(publicaciones);
    }

    public IActionResult Siguiendo()
    {
        var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

        HttpContext.Session.Remove("OtherUser");

        if (currentUser == null)
        {
            return RedirectToAction("Login", "Inicio");
        }

        var publicaciones = repo.GetAllPublicacionesSeguidosExceptoUsuario(currentUser.Username);

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




}
