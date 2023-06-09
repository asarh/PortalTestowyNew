﻿using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalR.API.Data;
using PortalR.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace PortalR.API.Controllers
{
    [Authorize] // w tym momencie do wszystkich akcji ValueContorller mają dostęp tylko za autoryzowani uzytkownicy(zalogowani)
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        private readonly DataContext _context;
      //  public interface IDbAsyncEnumerable;

        public ValuesController(DataContext context)
        {
            _context = context;
        }
        
        // GET api/values
        [HttpGet]
        public async Task<IActionResult>GetValuses()
        {
            var values =  await _context.Values.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5
        [AllowAnonymous] // do tych wartosci może się dostać każdy nawet nie zalogowany użytkownik
        [HttpGet("{id}")]
        public async Task<IActionResult>GetValue(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> AddValue([FromBody] Value value)
        {
            _context.Values.Add(value);
           await _context.SaveChangesAsync();
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> EditValue(int id, [FromBody] Value value)
        {
            var data = await _context.Values.FindAsync(id);
            data.Name = value.Name;
            _context.Values.Update(data);
            await _context.SaveChangesAsync();
            return Ok(data);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteValue(int id)
        {
            var data = await _context.Values.FindAsync(id);
            if (data == null)
                return NoContent();

            _context.Values.Remove(data);
                await _context.SaveChangesAsync();
            return Ok(data);

        }
    }
}
