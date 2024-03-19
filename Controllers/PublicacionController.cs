using Microsoft.AspNetCore.Mvc;
using RedSocialNetCore.Extensions;
using RedSocialNetCore.Helpers;
using RedSocialNetCore.Models;
using RedSocialNetCore.Repositories;

namespace RedSocialNetCore.Controllers
{
    public class PublicacionController : Controller
    {

        private RepositoryPublicacion repo;
        private HelperPathProvider helper;

        public PublicacionController(RepositoryPublicacion repo, HelperPathProvider helper)
        {
            this.repo = repo;
            this.helper = helper;

        }


        public IActionResult Publicar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Publicar(Publicacion publicacion, IFormFile imagen)
        {
            var currentUser = HttpContext.Session.GetObject<Usuario>("CurrentUser");

            // Crear una nueva publicación
            Publicacion nuevaPublicacion = new Publicacion
            {
                Texto = string.IsNullOrEmpty(publicacion.Texto) ? "" : publicacion.Texto,
                FechaPublicacion = DateTime.Now, // Asignar la fecha actual
                Username = currentUser.Username // Asignar el nombre de usuario actual
            };

            // Verificar si se proporciona una imagen
            if (imagen != null && imagen.Length > 0)
            {
                // Obtener el siguiente ID de la publicación para nombrar el archivo de imagen
                int nextId = repo.GetNextPublicacionId();

                // Generar el nombre del archivo de imagen utilizando el siguiente ID de la publicación
                string fileName = "imagen" + nextId.ToString() + ".jpeg";

                // Guardar la imagen en el sistema de archivos
                string path = this.helper.MapPath(fileName, Folders.Publicaciones);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                // Asignar el nombre de la imagen a la nueva publicación
                nuevaPublicacion.Imagen = fileName;
                nuevaPublicacion.TipoPublicacion = 2; // Tipo de publicación con imagen
            }
            else
            {
                nuevaPublicacion.Imagen = ""; // Asignar una cadena vacía al campo de imagen
                nuevaPublicacion.TipoPublicacion = 1; // Tipo de publicación sin imagen
            }

            nuevaPublicacion.FotoPerfil = currentUser.FotoPerfil;

            // Guardar la nueva publicación en la base de datos
            await this.repo.AddPublicacion(nuevaPublicacion);

            // Redirigir a la página de inicio después de publicar la publicación
            return RedirectToAction("Index", "Home");
        }




    }
}
