using Microsoft.AspNetCore.Mvc;
using PortalR.API.Data;
using PortalR.API.Dtos;
using PortalR.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace PortalR.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase    // Dziedziczenie po Controller wspiera również widoki ale ja nie używam widoków tylko angulara, wiec wystarczy ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(/*[FromBody]*/UserForRegisterDto userForRegisterDto) //API Controller nie potrzebuje FromBody ani sprawdzac stanu modelu ponieważ pobiera tylko dane
        {

            //if (!ModelState.IsValid)
              //return BadRequest(ModelState);
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _authRepository.UserExist(userForRegisterDto.Username))
                return BadRequest("Użytkownik o takiej nazwie już istnieje !");

            var userToCreate = new User
            {
                UserName = userForRegisterDto.Username
            };

            var createdUser = await _authRepository.Register(userToCreate, userForRegisterDto.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userfromRepo = await _authRepository.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

            if (userfromRepo == null)
                return Unauthorized();

            //Tworzymy Token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userfromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userfromRepo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); //generujemy poswiadczenia, dane uwierzytelniajace
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = creds//waznosc tokena ustwiony aktualnie na 12 godzin
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor); //utworzony token
            return Ok(new { token = tokenHandler.WriteToken(token) }); //token zostaje zwrócony do użytkownika i każde kolejne żądanie uzytkownika nie wymaga już logowania tylko sprawdzany jest token
        }
    }
}

//TODO Użycie uwierzytelniania