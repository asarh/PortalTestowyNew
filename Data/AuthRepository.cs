using PortalR.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

using Microsoft.EntityFrameworkCore;


namespace PortalR.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        #region method public

        public AuthRepository(DataContext context)                         //dodawanie konstruktora ctor enter
        {
            _context = context;
        }
        
        public  async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username); //pobranie użytkownika z bazy
            if (user == null)
                return null;

            if(!VerifyPassworddHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

            return user;


        }



        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePassworddHashSalt(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<bool> UserExist(string username)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == username))
                return true;

            return false;
        }

        #endregion

        #region method private

        private void CreatePassworddHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
                
        }


        private bool VerifyPassworddHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for(int i = 0; i <computedHash.Length; i++ )
                {
                    if (computedHash[i] != passwordHash[i]) // sprawdzam czy mój password(czyli za hashowane hasło) zgadza sie z paswordHash
                        return false;
                }
                return true; 
            }
        }

        #endregion
    }
}
