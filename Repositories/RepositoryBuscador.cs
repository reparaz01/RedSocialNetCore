using Microsoft.EntityFrameworkCore;
using RedSocialNetCore.Data;
using RedSocialNetCore.Models;

namespace RedSocialNetCore.Repositories
{
    public class RepositoryBuscador
    {
        private ContextApp context;

        public RepositoryBuscador(ContextApp context)
        {
            this.context = context;
        }

        public List<Usuario> BuscarUsuarios(string query)
        {
            var usuariosEncontrados = context.Usuarios
                .Where(u => u.Nombre.Contains(query) || u.Username.Contains(query))
                .Select(u => new Usuario { Username = u.Username, FotoPerfil = u.FotoPerfil })
                .ToList();

            return usuariosEncontrados;
        }


    }
}
