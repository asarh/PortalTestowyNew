using PortalR.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

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
        
        public Task<User> Login(string username, string password)
        {
            
            
            throw new NotImplementedException();
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
        public Task<bool> UserExist(string username)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
