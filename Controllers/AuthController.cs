using Microsoft.AspNetCore.Mvc;
using PortalR.API.Data;
using PortalR.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalR.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase    // Dziedziczenie po Controller wspiera również widoki ale ja nie używam widoków tylko angulara, wiec wystarczy ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(string username, string passwprd)
        {
            username = username.ToLower();
            if (await _authRepository.UserExist(username))
                return BadRequest("Użytkownik o takiej nazwie już istnieje !");

            var userToCreate = new User
            {
                UserName = username
            };

            var createdUser = await _authRepository.Register(userToCreate, passwprd);
            return StatusCode(201);
        }
    }
}
