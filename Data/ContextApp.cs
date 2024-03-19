using Microsoft.EntityFrameworkCore;
using RedSocialNetCore.Models;

namespace RedSocialNetCore.Data
{
    public class ContextApp : DbContext
    {
        public ContextApp(DbContextOptions<ContextApp> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Publicacion> Publicaciones { get; set; }
        public DbSet<Seguidores> Seguidores { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de claves primarias compuestas para la clase Like
            modelBuilder.Entity<Like>().HasKey(l => new { l.IdPublicacion, l.Username });
        }


    }
}