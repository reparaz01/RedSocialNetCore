using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RedSocialNetCore.Data;
using RedSocialNetCore.Models;
using RedSocialNetCore.Helpers;

namespace RedSocialNetCore.Repositories
{
    public class RepositoryInicio
    {
        private ContextApp context;

        public RepositoryInicio(ContextApp context)
        {
            this.context = context;
        }


        public async Task<bool> LogInUserAsync(string username, string password)
        {
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);

            if (usuario != null)
            {
                string salt = usuario.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = usuario.Password;
                bool response = HelperCryptography.CompareArrays(temp, passUser);

                return response;
            }
            else
            {
                return false;
            }
        }


        public Usuario GetUser(string username)
        {
            var usuario = (from u in context.Usuarios
                           where u.Username == username
                           select u).FirstOrDefault();

            return usuario;
        }

        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in context.Usuarios
                           select datos;
            return consulta.ToList();
        }

        public bool UsuarioExists(string username)
        {
            var consulta = from u in context.Usuarios
                           where u.Username == username
                           select u;

            return consulta.Any();
        }

        public bool EmailExists(string email)
        {
            var consulta = from u in context.Usuarios
                           where u.Email == email
                           select u;

            return consulta.Any();
        }

        public async Task<Usuario> RegisterUserAsync(string username, string password, string email, string nombre)
        {
            Usuario user = new Usuario();

            user.Username = username;
            user.Nombre = nombre;
            user.Email = email;
            user.Descripcion = "";
            user.FotoPerfil = "";
            user.Telefono = "";
            user.Rol = 2;

            //CADA USUARIO TENDRA UN SALT DISTINTO
            user.Salt = HelperCryptography.GenerateSalt();
            //GUARDAMOS EL PASSWORD EN BYTE[]
            user.Password =
                HelperCryptography.EncryptPassword(password, user.Salt);

            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();
            return user;
        }






    }
}