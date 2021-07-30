using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Resteraunt.Data;
using Resteraunt.DTOs;
using Resteraunt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resteraunt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MenuItemController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET: api/<MenuItemController>
        [HttpGet]
        public ActionResult Get()
        {
            var list = _context.MenuItems.ToList();
            return Ok(list);
        }

        // GET api/<MenuItemController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var menuItem = _context.MenuItems.FirstOrDefault(m => m.Id == id);

            if (menuItem != null)
            {
                return Ok(menuItem);
            }
            return NotFound();
        }

        // POST api/<MenuItemController>
        [HttpPost]
        public ActionResult Post(MenuItemCreateDto value)
        {
            MenuItem newMenuItem = _mapper.Map<MenuItem>(value);
            _context.MenuItems.Add(newMenuItem);
            _context.SaveChanges();
            return Ok(newMenuItem);
        }

        // PUT api/<MenuItemController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, MenuItemCreateDto value)
        {
            MenuItem menuFromDb = _context.MenuItems.FirstOrDefault(m => m.Id == id);

            if (menuFromDb == null) return NotFound();
           
            _context.SaveChanges();
            _mapper.Map(value, menuFromDb);
            
            return NoContent();
        }

        // DELETE api/<MenuItemController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            MenuItem menuFromDb = _context.MenuItems.FirstOrDefault(m => m.Id == id);
            
            if (menuFromDb == null) return NotFound();

            _context.MenuItems.Remove(menuFromDb);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
