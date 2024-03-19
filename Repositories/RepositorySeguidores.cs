using RedSocialNetCore.Data;
using RedSocialNetCore.Models;

namespace RedSocialNetCore.Repositories
{
    public class RepositorySeguidores
    {
        private ContextApp context;

        public RepositorySeguidores(ContextApp context)
        {
            this.context = context;
        }

        public List<Usuario> GetSeguidores(string username)
        {
            var seguidores = (from s in context.Seguidores
                              join u in context.Usuarios on s.SeguidorUsername equals u.Username
                              where s.SeguidoUsername == username
                              select new Usuario
                              {
                                  Username = u.Username,
                                  FotoPerfil = u.FotoPerfil
                              }).ToList();

            return seguidores;
        }

        public List<Usuario> GetSeguidos(string username)
        {
            var seguidos = (from s in context.Seguidores
                            join u in context.Usuarios on s.SeguidoUsername equals u.Username
                            where s.SeguidorUsername == username
                            select new Usuario
                            {
                                Username = u.Username,
                                FotoPerfil = u.FotoPerfil
                            }).ToList();

            return seguidos;
        }
    }
}
